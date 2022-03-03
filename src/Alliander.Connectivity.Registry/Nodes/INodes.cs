using Alliander.Connectivity.Registry.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alliander.Connectivity.Registry
{
    public interface INodes
    {
        public void CreateNode(string label);

        public Node GetNode(long id);

        public void UpdateNodes(Node nodeFrom, Node nodeTo, Relationships relationship);

        public string GetAllNodes();

        public string GetTopologyNodes(long id);

        public string GetNodesByLayer(string label);

        public List<Node> GetNodes();
    }
}
