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
    /// <summary>
    /// Api endpoints for relationships.
    /// </summary>
    public class HttpTriggerConnectivityRegisterRelationships
    {
        /// <summary>
        /// Constructor for <see cref="HttpTriggerConnectivityRegisterRelationships" />
        /// </summary>
        /// <param name="nodes">Instance of <see cref="INodes"/></param>
        /// <param name="relationship">Instance of <see cref="IRelationship"/></param>
        public HttpTriggerConnectivityRegisterRelationships(INodes nodes, IRelationship relationship)
        {
            this.nodes = nodes;
            this.relationship = relationship;
        }

        private readonly INodes nodes;
        private readonly IRelationship relationship;


        /// <summary>
        /// Creates a single relationship in memory.
        /// </summary>
        /// <param name="req">Incoming request.</param>
        /// <param name="nodeFrom">Id of the node of one side of the relationship.</param>
        /// <param name="nodeTo">Id of the node of the other side of the relationship.</param>
        /// <returns>An <see cref="IActionResult" />.</returns>
        [FunctionName(nameof(HttpTriggerConnectivityRegisterRelationships.PutRelationshipByNodeIds))]
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
    }
}

