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
    public class HttpTriggerConnectivityRegisterContainerRecipe
    {
        public HttpTriggerConnectivityRegisterContainerRecipe(INodes nodes, IRelationship relationship, IContainerRecipe containerRecipe)
        {
            this.nodes = nodes;
            this.relationship = relationship;
            this.containerRecipe = containerRecipe;
        }

        private readonly INodes nodes;
        private readonly IRelationship relationship;
        private readonly IContainerRecipe containerRecipe;

        /// <summary>
        /// Create a connectivitygraph as shown in Assignment 
        /// Create four asset and four topology node. 
        /// Create four relation between asset and topology node (Topology-asset relationship)
        /// Create three relation between topology node (topology relation).
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterContainerRecipe.PutCreateGraph))]
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
        /// Put densify collection of nodes to a container.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterContainerRecipe.PutContainerRecipe))]
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

