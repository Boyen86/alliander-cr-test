using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alliander.Connectivity.Registry.Test
{
    [TestClass]
    public class Registry
    {
        [TestMethod]
        public void CreateNode()
        {
            Nodes nodesRepository = new Nodes();
            var node = nodesRepository.CreateNode("test");

            Assert.AreEqual(0, node.Id);
            Assert.AreEqual("test", node.Label);
        }
    }
}
