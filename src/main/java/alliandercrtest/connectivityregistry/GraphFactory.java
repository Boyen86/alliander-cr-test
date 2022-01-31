package alliandercrtest.connectivityregistry;

public class GraphFactory {
    NodeFactory nodeFactory;
    RelationshipFactory relationshipFactory;

    public GraphFactory(NodeFactory nodeFactory, RelationshipFactory relationshipFactory) {
        this.nodeFactory = nodeFactory;
        this.relationshipFactory = relationshipFactory;
    }
    protected ConnectivityGraph createSimpleGraph() {
		ConnectivityGraph graph = new ConnectivityGraph();

        TopologyNode topologyNode1 = createConnectedTopologyAndAssetNode(graph, "veld");
        graph.registerRelationship(relationshipFactory.createOutgoingTopologyRelationship(topologyNode1));

        TopologyNode topologyNode2 = createConnectedTopologyAndAssetNode(graph, "rail");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode1, topologyNode2));

        TopologyNode topologyNode3 = createConnectedTopologyAndAssetNode(graph, "kabel");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode2, topologyNode3));

        TopologyNode topologyNode4 = createConnectedTopologyAndAssetNode(graph, "transformator");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode3, topologyNode4));
        graph.registerRelationship(relationshipFactory.createOutgoingTopologyRelationship(topologyNode4));

		return graph;
    }

    public ConnectivityGraph createComplexGraph() {
		ConnectivityGraph graph = new ConnectivityGraph();

        TopologyNode topologyNode1 = createConnectedTopologyAndAssetNode(graph, "veld");
        graph.registerRelationship(relationshipFactory.createOutgoingTopologyRelationship(topologyNode1));

        TopologyNode topologyNode2 = createConnectedTopologyAndAssetNode(graph, "rail");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode1, topologyNode2));

        TopologyNode topologyNode3 = createConnectedTopologyAndAssetNode(graph, "kabel");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode2, topologyNode3));

        // First branch
        TopologyNode topologyNode4 = createConnectedTopologyAndAssetNode(graph, "transformator");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode3, topologyNode4));
        
        TopologyNode topologyNode5 = createConnectedTopologyAndAssetNode(graph, "transformator");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode4, topologyNode5));
        graph.registerRelationship(relationshipFactory.createOutgoingTopologyRelationship(topologyNode5));

        // Second branch
        TopologyNode topologyNode6 = createConnectedTopologyAndAssetNode(graph, "transformator");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode3, topologyNode6));
        
        TopologyNode topologyNode7 = createConnectedTopologyAndAssetNode(graph, "transformator");
        graph.registerRelationship(relationshipFactory.createRelationship(topologyNode6, topologyNode7));
        graph.registerRelationship(relationshipFactory.createOutgoingTopologyRelationship(topologyNode7));

        return graph;
    }

    private TopologyNode createConnectedTopologyAndAssetNode(ConnectivityGraph graph, String assetLabel) {
        AssetNode asset = nodeFactory.createAssetNode(assetLabel);
        graph.registerNode(asset);
        TopologyNode topology = nodeFactory.createTopologyNode();
        graph.registerNode(topology);
        graph.registerRelationship(relationshipFactory.createRelationship(asset, topology));

        return topology;
    }
}
