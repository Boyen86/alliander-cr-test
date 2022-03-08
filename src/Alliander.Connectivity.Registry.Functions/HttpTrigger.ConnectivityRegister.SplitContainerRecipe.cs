using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Alliander.Connectivity.Registry.Functions
{
    public class HttpTriggerConnectivityRegisterSplitContainer
    {
        public HttpTriggerConnectivityRegisterSplitContainer(INodes nodes, IRelationship relationship, ISplitContainer splitContainer)
        {
            this.nodes = nodes;
            this.relationship = relationship;
            this.splitContainer = splitContainer;
        }

        private readonly INodes nodes;
        private readonly IRelationship relationship;
        private readonly ISplitContainer splitContainer;

        /// <summary>
        /// Executes the split container densify and returns the result.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterSplitContainer.SplitContainerRecipe))]
        [OpenApiOperation(
            tags: new[] { "SplitContainer" },
            operationId: "Get split container densify.",
            Summary = "Executes the split container densify and returns the result.",
            Description = "Executes the split container densify and returns the result.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<string> SplitContainerRecipe(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SplitContainerRecipe")]
            HttpRequest req)
        {
            return await Task.FromResult(splitContainer.SplitContainerRecipe(nodes));
        }


        /// <summary>
        /// Create a connectivitygraph as shown in Assignment (First image)
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterSplitContainer.PutCreateGraphExample1))]
        [OpenApiOperation(
            tags: new[] { "SplitContainer" },
            operationId: "PutCreateGraphExample1",
            Summary = "Save the asset and topology nodes and relationships as shown in the assignment first image.",
            Description = "Create a connectivitygraph by saving nodes and relationships.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutCreateGraphExample1(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "PutCreateGraphExample1")]
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
        /// Create a connectivitygraph as shown in Assignment (second image)
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterSplitContainer.PutCreateGraphExample2))]
        [OpenApiOperation(
            tags: new[] { "SplitContainer" },
            operationId: "PutCreateGraphExample2",
            Summary = "Save the asset and topology nodes and relationships as shown in the assignment second image.",
            Description = "Create a connectivitygraph by saving nodes and relationships.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutCreateGraphExample2(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "PutCreateGraphExample2")]
            HttpRequest req)
        {
            nodes.CreateNode("veld");
            nodes.CreateNode("rail");
            nodes.CreateNode("kabel");
            nodes.CreateNode("transformator");
            nodes.CreateNode("rail");
            nodes.CreateNode("kabel");
            nodes.CreateNode("transformator");

            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");

            relationship.CreateRelationship(0, 7, nodes);
            relationship.CreateRelationship(1, 8, nodes);
            relationship.CreateRelationship(2, 9, nodes);
            relationship.CreateRelationship(3, 10, nodes);
            relationship.CreateRelationship(4, 11, nodes);
            relationship.CreateRelationship(5, 12, nodes);
            relationship.CreateRelationship(6, 13, nodes);

            relationship.CreateRelationship(7, 8, nodes);
            relationship.CreateRelationship(8, 9, nodes);
            relationship.CreateRelationship(9, 10, nodes);
            relationship.CreateRelationship(9, 12, nodes);
            relationship.CreateRelationship(10, 11, nodes);
            relationship.CreateRelationship(12, 13, nodes);

            return await Task.FromResult(new OkResult());
        }

        /// <summary>
        /// Create a connectivitygraph as shown in Assignment (third image)
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterSplitContainer.PutCreateGraphExample3))]
        [OpenApiOperation(
            tags: new[] { "SplitContainer" },
            operationId: "PutCreateGraphExample3",
            Summary = "Save the asset and topology nodes and relationships as shown in the assignment third image.",
            Description = "Create a connectivitygraph by saving nodes and relationships.",
            Visibility = OpenApiVisibilityType.Important)]
        public async Task<IActionResult> PutCreateGraphExample3(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "PutCreateGraphExample3")]
            HttpRequest req)
        {
            nodes.CreateNode("veld");
            nodes.CreateNode("rail");
            nodes.CreateNode("kabel");
            nodes.CreateNode("transformator");
            nodes.CreateNode("rail");
            nodes.CreateNode("kabel");
            nodes.CreateNode("transformator");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");
            nodes.CreateNode("topology");

            relationship.CreateRelationship(0, 7, nodes);
            relationship.CreateRelationship(1, 8, nodes);
            relationship.CreateRelationship(2, 9, nodes);
            relationship.CreateRelationship(3, 10, nodes);
            relationship.CreateRelationship(4, 11, nodes);
            relationship.CreateRelationship(5, 12, nodes);
            relationship.CreateRelationship(6, 13, nodes);

            relationship.CreateRelationship(7, 8, nodes);
            relationship.CreateRelationship(8, 9, nodes);
            relationship.CreateRelationship(9, 10, nodes);
            relationship.CreateRelationship(10, 11, nodes);
            relationship.CreateRelationship(12, 13, nodes);
            relationship.CreateRelationship(13, 9, nodes);

            return await Task.FromResult(new OkResult());
        }
    }
}

