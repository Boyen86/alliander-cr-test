using Alliander.Connectivity.Registry.Domain;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Alliander.Connectivity.Registry
{
    public class ContainerRecipe : IContainerRecipe
    {
        public ContainerRecipe()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Densify the topology of the nodes.
        /// </summary>
        /// <param name="node">Instance of <see cref="INodes"/></param>
        /// <returns></returns>
        public string Densify(INodes node)
        {
            var nodes = node.GetNodes().Where(x => x.Label != "topology").ToList();
            var containerNode = CreateContainerNode(nodes.Max(x => x.Id));

            var assetNodes = CreateNodes(nodes);

            createRelationships(assetNodes, containerNode);
            assetNodes.Add(containerNode);

            var allNodes = assetNodes.Select(x => new
            {
                x.Id,
                x.Label,
                RelationshipFrom = x.Relationship
                .Select(y => new { y.NodeFrom.Id, y.NodeFrom.Label, }),
                RelationshipTo = x.Relationship
                .Select(y => new { y.NodeTo.Id, y.NodeTo.Label })
            }).ToList();

            return JsonConvert.SerializeObject(allNodes);
        }

        /// <summary>
        /// Adds the assetnodes in the topology.
        /// </summary>
        /// <param name="assetNodes">Nodes <see cref="Node"/> of the type assets.</param>
        /// <returns>List of asset nodes <see cref="Node"/></returns>
        private List<Node> CreateNodes(List<Node> assetNodes)
        {
            var nodes = new List<Node>();
            foreach (var node in assetNodes)
            {
                var newNode = new Node()
                {
                    Id = node.Id,
                    Label = node.Label
                };
                nodes.Add(newNode);
            }
            return nodes;
        }

        /// <summary>
        /// Create a new container node <see cref="Node"/>.
        /// </summary>
        /// <param name="id">Identifier of the node</param>
        /// <returns>A container node <see cref="Node"/></returns>
        private Node CreateContainerNode(long id)
        {
            return new Node() { Id = Interlocked.Increment(ref id), Label = "container", Relationship = new List<Relationships>() };
        }

        /// <summary>
        /// Creates relationships between asset and container node.
        /// </summary>
        /// <param name="assetNodes">List of asset nodes <see cref="Node"/></param>
        /// <param name="containerNode">A container node <see cref="Node"/></param>
        private void createRelationships(List<Node> assetNodes, Node containerNode)
        {
            var index = 0;
            foreach (var node in assetNodes)
            {
                var listRelationships = new List<Relationships>();
                var relationship = new Relationships()
                {
                    Id = index,
                    Label = "DESCRIBES_ASSET",
                    NodeFrom = node,
                    NodeTo = containerNode
                };
                listRelationships.Add(relationship);

                var relationshipContainerNode = containerNode.Relationship.ToList();
                relationshipContainerNode.Add(relationship);
                containerNode.Relationship = relationshipContainerNode;
                 node.Relationship = listRelationships;

                index++;

            }
        }
    }
}
