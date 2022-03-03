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

        private Node CreateContainerNode(long id)
        {
            return new Node() { Id = Interlocked.Increment(ref id), Label = "container", Relationship = new List<Relationships>() };
        }

        private void createRelationships(List<Node> outputNodes, Node containerNode)
        {
            var index = 0;
            foreach (var node in outputNodes)
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
