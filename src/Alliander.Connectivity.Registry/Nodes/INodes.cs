using Alliander.Connectivity.Registry.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alliander.Connectivity.Registry
{
    public interface INodes
    {
        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="label">Label of the node.</param>
        public Node CreateNode(string label);

        /// <summary>
        /// Gets the node by identifier.
        /// </summary>
        /// <param name="id">Identifier of the node.</param>
        /// <returns>A node <see cref="Node"/> <</returns>
        public Node GetNode(long id);

        /// <summary>
        /// Updates relationships of the node.
        /// </summary>
        /// <param name="nodeFrom">Node of the start.</param>
        /// <param name="nodeTo">Node of the end.</param>
        /// <param name="relationship">Relationship of the nodes.</param>
        public void UpdateRelationshipOfNodes(Node nodeFrom, Node nodeTo, Relationships relationship);

        /// <summary>
        /// Gets all node in the graph.
        /// </summary>
        /// <returns>String in json format of all nodes in the graph.</returns>
        public string GetAllNodes();

        /// <summary>
        /// Get a topology node by identifier.
        /// </summary>
        /// <param name="id">Identifier of the node.</param>
        /// <returns>String in json format of the topology node.</returns>
        public string GetTopologyNodes(long id);

        /// <summary>
        /// Get nodes in the graph by layer.
        /// </summary>
        /// <param name="label">Label of node.</param>
        /// <returns>String in json format of nodes in the graph by given layer</returns>
        public string GetNodesByLayer(string label);

        /// <summary>
        /// Get all nodes in the graph.
        /// </summary>
        /// <returns>List of <see cref="Node" /> in the graph</returns>
        public List<Node> GetNodes();

        /// <summary>
        /// Gets the available identifier to save a node.
        /// </summary>
        /// <returns>Identifier in format long</returns>
        public long GetIdOfNode();
    }
}
