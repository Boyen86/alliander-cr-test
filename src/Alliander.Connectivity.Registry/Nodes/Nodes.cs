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
        private long id;

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="label">Label of the node.</param>
        public Node CreateNode(string label)
        {
            var newNode = new Node();
            if (!_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                outputNodes = new List<Node>();
                newNode.Id = 0;
                newNode.Label = label;
                outputNodes.Add(newNode);
            }
            else
            {
                newNode.Id = GetIdOfNode();
                newNode.Label = label;
                
                outputNodes.Add(newNode);
            }
            _cache.Set("nodes", outputNodes, _cacheTimeout);
            return newNode;
        }

        /// <summary>
        /// Gets the node by identifier.
        /// </summary>
        /// <param name="id">Identifier of the node.</param>
        /// <returns>A node <see cref="Node"/> <</returns>
        public Node GetNode(long id)
        {
            if (!_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                return new Node();
            }
            return outputNodes.Where(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Updates relationships of the node.
        /// </summary>
        /// <param name="nodeFrom">Node of the start.</param>
        /// <param name="nodeTo">Node of the end.</param>
        /// <param name="relationship">Relationship of the nodes.</param>
        public void UpdateRelationshipOfNodes(Node nodeFrom, Node nodeTo, Relationships relationship)
        {
            if (_cache.TryGetValue("nodes", out List<Node> outputNodes))
            {
                UpdateRelationshipNode(nodeFrom, relationship, outputNodes);
                UpdateRelationshipNode(nodeTo, relationship, outputNodes);
                _cache.Set("nodes", outputNodes, _cacheTimeout);
            }
        }

        /// <summary>
        /// Gets all node in the graph.
        /// </summary>
        /// <returns>String in json format of all nodes in the graph.</returns>
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

        /// <summary>
        /// Get a topology node by identifier.
        /// </summary>
        /// <param name="id">Identifier of the node.</param>
        /// <returns>String in json format of the topology node.</returns>
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

        /// <summary>
        /// Get nodes in the graph by layer.
        /// </summary>
        /// <param name="label">Label of node.</param>
        /// <returns>String in json format of nodes in the graph by given layer</returns>
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

        /// <summary>
        /// Get all nodes in the graph.
        /// </summary>
        /// <returns>List of <see cref="Node" /> in the graph</returns>
        public List<Node> GetNodes()
        {
            return (List<Node>)_cache.Get("nodes");
        }

        /// <summary>
        /// Gets the available identifier to save a node.
        /// </summary>
        /// <returns>Identifier in format long</returns>
        public long GetIdOfNode()
        {
            if (id == 0)
            {
                var nodes = JsonConvert.DeserializeObject<IEnumerable<Node>>(GetNodesByLayer("veld"));
                var maxId = nodes.Max(x => x.Id);
                id = Interlocked.Increment(ref maxId);
                return id;
            }
            id = Interlocked.Increment(ref id);
            return id;
        }

        /// <summary>
        /// Get all nodes in the graph.
        /// </summary>
        /// <returns>List of <see cref="Node" /> in the graph</returns>
        private List<Node> GetNodes(long id, List<Node> nodes, Node node)
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

        /// <summary>
        /// Updates relationships of the node.
        /// </summary>
        /// <param name="node">A node <see cref="Node"/></param>
        /// <param name="relationship">A relationship <see cref="Relationships"/></param>
        /// <param name="outputNodes">List of <see cref="Relationships"/></param>
        private void UpdateRelationshipNode(Node node, Relationships relationship, List<Node> outputNodes)
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
