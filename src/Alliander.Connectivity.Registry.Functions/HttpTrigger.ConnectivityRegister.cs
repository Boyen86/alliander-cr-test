using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Alliander.Connectivity.Registry.Functions
{
    public class HttpTrigger
    {
        public HttpTrigger(INodes nodes, IRelationship relationship, IContainerRecipe containerRecipe)
        {
            this.nodes = nodes;
            this.relationship = relationship;
            this.containerRecipe = containerRecipe;
        }

        private readonly INodes nodes;
        private readonly IRelationship relationship;
        private readonly IContainerRecipe containerRecipe;

        /// <summary>
        /// Creates a single node in memory.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="label">Label of the node.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTrigger.PutNodeByLabel))]
        [OpenApiOperation(
            tags: new[] { "Node" },
            operationId: "putNodeByLabel",
            Summary = "Put the node by label.",
            Description = "Puts the node.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(
            name: "label",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(string),
            Summary = "Label of the Node",
            Description = "Label of the Node to create.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutNodeByLabel(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Node/{label}")]
            HttpRequest req, string label)
        {
            nodes.CreateNode(label);
            return await Task.FromResult(new OkResult());
        }

        /// <summary>
        /// Creates a single relationship in memory.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="nodeFrom">Id of the node of one side of the relationship.</param>
        /// <param name="nodeTo">Id of the node of the other side of the relationship.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTrigger.PutRelationshipByNodeIds))]
        [OpenApiOperation(
            tags: new[] { "Relationship" },
            operationId: "PutRelationshipByNodeIds",
            Summary = "Put the relationship by node ids.",
            Description = "Puts the relationship.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(
            name: "nodeFrom",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(long),
            Summary = "Id of the node.",
            Description = "Id of the Node.",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(
            name: "nodeTo",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(long),
            Summary = "Id of the node.",
            Description = "Id of the Node.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutRelationshipByNodeIds(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Relationship/{nodeFrom}/{nodeTo}")]
            HttpRequest req, long nodeFrom, long nodeTo)
        {
            relationship.CreateRelationship(nodeFrom, nodeTo, nodes);
            return await Task.FromResult(new OkResult());
        }

        /// <summary>
        /// Create a connectivitygraph as shown in Assignment 
        /// Create four asset and four topology node. 
        /// Create four relation between asset and topology node (Topology-asset relationship)
        /// Create three relation between topology node (topology relation).
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTrigger.PutCreateGraph))]
        [OpenApiOperation(
            tags: new[] { "Node" },
            operationId: "PutNodes",
            Summary = "Save the asset and topology nodes and relationships as shown in the assignment.",
            Description = "Create a connectivitygraph by saving nodes and relationships.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutCreateGraph(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Connectivitygraph")]
            HttpRequest req)
        {
            nodes.CreateNode("veld");
            nodes.CreateNode("rail");
            nodes.CreateNode("kabel");
            nodes.CreateNode("transformator");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");

            relationship.CreateRelationship(0, 4, nodes);
            relationship.CreateRelationship(1, 5, nodes);
            relationship.CreateRelationship(2, 6, nodes);
            relationship.CreateRelationship(3, 7, nodes);
            relationship.CreateRelationship(4, 5, nodes);
            relationship.CreateRelationship(5, 6, nodes);
            relationship.CreateRelationship(6, 7, nodes);

            return await Task.FromResult(new OkResult());
        }

        /// <summary>
        /// Get all nodes of the connectivitygraph
        /// also the relationships of the node and connection to another node is shown
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="nodes" />.</returns>
        [FunctionName(nameof(HttpTrigger.GetNodes))]
        [OpenApiOperation(
            tags: new[] { "Connectivitygraph" },
            operationId: "GetNodes",
            Summary = "Get all nodes of the connectivitygraph",
            Description = "Get all nodes of the connectivitygraph.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<string> GetNodes(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Connectivitygraph")]
            HttpRequest req)
        {
            return await Task.FromResult(nodes.GetAllNodes());
        }

        /// <summary>
        /// Get topology node with his connections in this layer of the connectivity graph by id
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="nodeId">identifier of the node</param>
        /// <returns>An <see cref="nodes" />.</returns>
        [FunctionName(nameof(HttpTrigger.GetTopologyNodes))]
        [OpenApiOperation(
            tags: new[] { "Connectivitygraph" },
            operationId: "GetTopologyNodes",
            Summary = "Get topology nodes of the connectivity graph",
            Description = "Get topology nodes of the connectivity graph",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(
            name: "id",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(long),
            Summary = "Identifier of the node.",
            Description = "Identifier of the Node.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<string> GetTopologyNodes(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "TopologyNodes/{id}")]
            HttpRequest req, long id)
        {
            return await Task.FromResult(nodes.GetTopologyNodes(id));
        }

        /// <summary>
        /// Get nodes of a layer by label
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="nodeId">identifier of the node</param>
        /// <returns>An <see cref="nodes" />.</returns>
        [FunctionName(nameof(HttpTrigger.GetNodesOfLayer))]
        [OpenApiOperation(
            tags: new[] { "Connectivitygraph" },
            operationId: "GetNodesOfLayer",
            Summary = "Get nodes of a layer by label",
            Description = "Get nodes of a layer by label",
            Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(
            name: "label",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(string),
            Summary = "Label of the node.",
            Description = "Label of the Node.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<string> GetNodesOfLayer(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "TopologyNodes/{label}")]
            HttpRequest req, string label)
        {
            return await Task.FromResult(nodes.GetNodesByLayer(label));
        }


        /// <summary>
        /// Put densify collection of nodes to a container.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTrigger.PutContainerRecipe))]
        [OpenApiOperation(
            tags: new[] { "ContainerRecipe" },
            operationId: "PutContainerRecipe",
            Summary = "Put densify collection of nodes to a container.",
            Description = "Puts densify collection of nodes to a container.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<string> PutContainerRecipe(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "PutContainerRecipe")]
            HttpRequest req)
        {
            return await Task.FromResult(containerRecipe.Densify(nodes));
        }
    }
}

