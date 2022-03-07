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
    public class HttpTriggerConnectivityRegisterNodes
    {
        public HttpTriggerConnectivityRegisterNodes(INodes nodes, IRelationship relationship)
        {
            this.nodes = nodes;
            this.relationship = relationship;
        }

        private readonly INodes nodes;
        private readonly IRelationship relationship;

        /// <summary>
        /// Creates a single node in memory.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="label">Label of the node.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterNodes.PutNodeByLabel))]
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
        /// Get all nodes of the connectivitygraph
        /// also the relationships of the node and connection to another node is shown
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="nodes" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterNodes.GetNodes))]
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
        [FunctionName(nameof(HttpTriggerConnectivityRegisterNodes.GetTopologyNodes))]
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
        [FunctionName(nameof(HttpTriggerConnectivityRegisterNodes.GetNodesOfLayer))]
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
    }
}

