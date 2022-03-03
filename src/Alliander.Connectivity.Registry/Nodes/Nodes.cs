using Alliander.Connectivity.Registry.Domain;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alliander.Connectivity.Registry
{
    public class Nodes : INodes
    {
        public Nodes()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(5);
        private readonly string[] assetNodeLabel = { "veld", "rail", "kabel", "transformator" };

        public void CreateNode(string label)
        {
            if (!_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                outputNodes = new List<Node>();
                outputNodes.Add(new Node() { Id = 0, Label = label });
            }
            else
            {
                var id = outputNodes.Max(x => x.Id);
                outputNodes.Add(new Node() { Id = Interlocked.Increment(ref id), Label = label });
            }
            _cache.Set("nodes", outputNodes, _cacheTimeout);
        }

        public Node GetNode(long id)
        {
            if (!_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                return new Node();
            }
            return outputNodes.Where(x => x.Id == id).FirstOrDefault();
        }

        public void UpdateNodes(Node nodeFrom, Node nodeTo, Relationships relationship)
        {
            if (_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                UpdateRelationshipNode(nodeFrom, relationship, outputNodes);
                UpdateRelationshipNode(nodeTo, relationship, outputNodes);
                _cache.Set("nodes", outputNodes, _cacheTimeout);
            }
        }

        public string GetAllNodes()
        {
            var nodes = (List<Node>) _cache.Get("nodes");
            if(nodes == null)
            {
                return "No nodes in the memory";
            }
            var graph = nodes.Select(x => new
            {
                x.Id,
                x.Label,
                RelationshipFrom = x.Relationship
                .Select(y => new { y.NodeFrom.Id, y.NodeFrom.Label, }),
                RelationshipTo = x.Relationship
                .Select(y => new { y.NodeTo.Id, y.NodeTo.Label })
            }).ToList();

            return  JsonConvert.SerializeObject(graph);
        }

        public string GetTopologyNodes(long id)
        {
            var nodes = (List<Node>)_cache.Get("nodes");
            if (nodes == null)
            {
                return "No nodes in the memory";
            }
            var node = nodes.Where(x => x.Id == id).FirstOrDefault();
            if (node == null)
            {
                return "Node not found";
            }
            if (node.Label != "topology")
            {
                return "Given node is not of the kind topology";
            }

            var topologyNodes = GetNodes(id, nodes, node).Select(x=> new {x.Id, x.Label });

            return JsonConvert.SerializeObject(topologyNodes);
        }

        public string GetNodesByLayer(string label)
        {
            var nodes = (List<Node>)_cache.Get("nodes");
            if (nodes == null)
            {
                return "No nodes in the memory";
            }
            if (assetNodeLabel.Contains(label))
            {
                var layerNodes = nodes.Where(x => assetNodeLabel.Contains(x.Label)).Select(x => new { x.Id, x.Label });
                return JsonConvert.SerializeObject(layerNodes);
            }
            if (label == "topology")
            {
                var layerNodes = nodes.Where(x => x.Label == label).Select(x => new { x.Id, x.Label });
                return JsonConvert.SerializeObject(layerNodes);
            }
            return "Layer not found";

        }
        public List<Node> GetNodes()
        {
            return (List<Node>)_cache.Get("nodes");
        }

        private static List<Node> GetNodes(long id, List<Node> nodes, Node node)
        {
            var topologyNodes = new List<Node>();
            topologyNodes.Add(node);
            var relationships = node.Relationship.Where(x => x.Label == "CONNECTS_TOPOLOGY").ToList();
            foreach (var relationship in relationships)
            {
                var newNode = new Node();
                if (relationship.NodeFrom.Id != id)
                {
                    newNode = nodes.Where(x => x.Id == relationship.NodeFrom.Id).FirstOrDefault();
                }
                if (relationship.NodeTo.Id != id)
                {
                    newNode = nodes.Where(x => x.Id == relationship.NodeTo.Id).FirstOrDefault();
                }
                topologyNodes.Add(newNode);
            }
            return topologyNodes;
        }

        private static void UpdateRelationshipNode(Node node, Relationships relationship, List<Node> outputNodes)
        {
            var nodes = outputNodes.Where(x => x == node).FirstOrDefault();

            if (nodes.Relationship == null)
            {
                nodes.Relationship = new List<Relationships>();
            }
            var relationships = nodes.Relationship.ToList();
            relationships.Add(relationship);
            nodes.Relationship = relationships;
        }
    }
}
