using Alliander.Connectivity.Registry.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Alliander.Connectivity.Registry
{
    public class SplitContainer : ISplitContainer
    {
        private readonly string[] assetNodeLabel = { "veld", "rail", "kabel", "transformator" };
        private long relationID = 0;

        /// <summary>
        /// Performs a splitContainerRecipe that will densify a collection of nodes to a split container.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>Result in JSON format</returns>
        public string SplitContainerRecipe(INodes nodes)
        {

            var listTopologyNodes = nodes.GetNodes().Where(x => x.Label == "topology").ToList();
            var startNodes = GetStartNodes(listTopologyNodes);
            var splitContainerGraph = new List<Node>();

            for (; ; )
            {
                var splitNodes = new List<Node>();
                foreach (var node in startNodes)
                {
                    var topologyNode = node;
                    var topologyRelationshipsOfNode = node.Relationship.Where(x => x.Label == "CONNECTS_TOPOLOGY").ToList();
                    var assetNode = node.Relationship.Where(x => x.Label == "DESCRIBES_ASSET").FirstOrDefault().NodeFrom;
                    var containerNode = new Node() { Id = nodes.GetIdOfNode(), Label = "container", Relationship = new List<Relationships>() };

                    for (; ; )
                    {
                        if (IsSplitNode(topologyRelationshipsOfNode))
                        {
                            splitNodes.Add(topologyNode);
                            break;
                        }

                        RelationshipToContainerOrSplit(assetNode, containerNode);
                        splitContainerGraph.Add(assetNode);
                        topologyNode = topologyRelationshipsOfNode.Where(x => x.Label == "CONNECTS_TOPOLOGY" && x.NodeFrom == topologyNode).Select(x => x.NodeTo).FirstOrDefault();
                        if (topologyNode == null)
                        {
                            break;
                        }
                        topologyRelationshipsOfNode = topologyNode.Relationship.ToList();
                        assetNode = topologyNode.Relationship.Where(x => x.Label == "DESCRIBES_ASSET" && x.NodeTo == topologyNode).FirstOrDefault().NodeFrom;
                    }
                    splitContainerGraph.Add(containerNode);
                }

                splitNodes = splitNodes.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                startNodes = GetNodesAfterSplit(nodes, splitContainerGraph, splitNodes);

                if (startNodes.Count() == 0)
                {
                    break;
                }
            }

            CreateRelationsshipsContainerToSplit(splitContainerGraph);
            return JsonConvert.SerializeObject(splitContainerGraph);
        }

        /// <summary>
        /// Creates relationships between containers and split nodes.
        /// </summary>
        /// <param name="splitContainerGraph">The graph of nodes.</param>
        private void CreateRelationsshipsContainerToSplit(List<Node> splitContainerGraph)
        {
            var listSplitNodes = splitContainerGraph.Where(x => x.Label == "split").ToList();
            var listContainerNodes = splitContainerGraph.Where(x => x.Label == "container").ToList();
            foreach (var node in listContainerNodes)
            {
                foreach (var splitNode in listSplitNodes)
                {
                    var relationshipsSplitNode = splitNode.Relationship.ToList();
                    var relationshipsContainerNode = node.Relationship.ToList();
                    Relationships relationShip;

                    if (node.Id < splitNode.Id)
                    {
                        relationShip = new Relationships() { Id = Interlocked.Increment(ref relationID), Label = "CONNECTS_TOPOLOGY", NodeFrom = node, NodeTo = splitNode };
                    }
                    else
                    {
                        relationShip = new Relationships() { Id = Interlocked.Increment(ref relationID), Label = "CONNECTS_TOPOLOGY", NodeFrom = splitNode, NodeTo = node };
                    }
                    relationshipsSplitNode.Add(relationShip);
                    relationshipsContainerNode.Add(relationShip);
                    node.Relationship = relationshipsContainerNode;
                    splitNode.Relationship = relationshipsSplitNode;
                }
            }
        }


        /// <summary>
        /// Gets the nodes that comes after the split container.
        /// </summary>
        /// <param name="nodes"><paramref name="nodes"/></param>
        /// <param name="splitContainerGraph">Split container</param>
        /// <param name="splitNodes">Split nodes</param>
        /// <returns>List of nodes</returns>
        private List<Node> GetNodesAfterSplit(INodes nodes, List<Node> splitContainerGraph, List<Node> splitNodes)
        {
            List<Node> startNodes = new List<Node>();
            foreach (var splitNode in splitNodes)
            {
                foreach (var startNode in splitNode.Relationship.Where(x => x.NodeFrom == splitNode).Select(x => x.NodeTo))
                {
                    startNodes.Add(startNode);
                }

                var assetNode = splitNode.Relationship.Where(x => x.Label == "DESCRIBES_ASSET").Select(x => x.NodeFrom).FirstOrDefault();
                var containerNodes = splitContainerGraph.Where(x => x.Label == "container").ToList();

                splitNode.Id = nodes.GetIdOfNode();
                splitNode.Label = "split";
                splitNode.Relationship = new List<Relationships>();

                RelationshipToContainerOrSplit(assetNode, splitNode);
                splitContainerGraph.Add(assetNode);
                splitContainerGraph.Add(splitNode);
            }

            return startNodes;
        }

        /// <summary>
        /// Gets the nodes where the graph starts.
        /// </summary>
        /// <param name="listTopologyNodes">List of nodes in the graph.</param>
        /// <returns>List of nodes where the graph starts.</returns>
        private List<Node> GetStartNodes(List<Node> listTopologyNodes)
        {
            var startNodes = new List<Node>();
            foreach (var node in listTopologyNodes)
            {
                var relationshipNode = node.Relationship.Where(x => x.Label == "CONNECTS_TOPOLOGY").ToList();

                if (relationshipNode.Count() == 1 &&
                    node.Relationship.Where(x => x.NodeFrom == node).Count() > 0)
                {
                    startNodes.Add(node);
                }
            }

            return startNodes;
        }

        /// <summary>
        /// Checks if the node is of type split.
        /// </summary>
        /// <param name="relationships">Relationships of the node.</param>
        /// <returns>A boolean if the node is of type split.</returns>
        private bool IsSplitNode(IEnumerable<Relationships> relationships)
        {
            if (relationships.Count() > 3)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates relationships between asset and conatiner/split nodes.
        /// </summary>
        /// <param name="assetNode">Node of the type Asset.</param>
        /// <param name="node">Node of the type container or split.</param>
        private void RelationshipToContainerOrSplit(Node assetNode, Node node)
        {
            var assetNodeRelationship = new List<Relationships>();
            var containerNodeRelationships = node.Relationship.ToList();

            var relationship = new Relationships()
            {
                Id = relationID,
                Label = "DESCRIBES_ASSET",
                NodeFrom = assetNode,
                NodeTo = node
            };
            assetNodeRelationship.Add(relationship);
            containerNodeRelationships.Add(relationship);

            node.Relationship = containerNodeRelationships;
            assetNode.Relationship = assetNodeRelationship;

            relationID = Interlocked.Increment(ref relationID);
        }
    }
}
