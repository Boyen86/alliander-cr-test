package alliandercrtest.connectivityregistry;

import java.util.ArrayList;
import java.util.List;

public class ConnectivityGraph {
    // What is meant with the requirement "given a layer name you can easily find data on that layer"?
    
    private long idCounter = 0;

    protected List<Node> nodes = new ArrayList<>();
    protected List<Relationship> relationships = new ArrayList<>();

    public void initializeSimpleGraph() {
        AssetNode assetNode1 = this.createAssetNode("veld");
        TopologyNode topologyNode1 = this.createTopologyNode();
        this.connectNodes(assetNode1, topologyNode1);
        this.addOutgoingTopologyRelationship(topologyNode1);

        AssetNode assetNode2 = this.createAssetNode("rail");
        TopologyNode topologyNode2 = this.createTopologyNode();
        this.connectNodes(assetNode2, topologyNode2);
        this.connectNodes(topologyNode1, topologyNode2);

        AssetNode assetNode3 = this.createAssetNode("kabel");
        TopologyNode topologyNode3 = this.createTopologyNode();
        this.connectNodes(assetNode3, topologyNode3);
        this.connectNodes(topologyNode2, topologyNode3);

        AssetNode assetNode4 = this.createAssetNode("transformator");
        TopologyNode topologyNode4 = this.createTopologyNode();
        this.connectNodes(assetNode4, topologyNode4);
        this.connectNodes(topologyNode3, topologyNode4);
        this.addOutgoingTopologyRelationship(topologyNode4);
    }

    public void initializeComplexGraph() {
        AssetNode assetNode1 = this.createAssetNode("veld");
        TopologyNode topologyNode1 = this.createTopologyNode();
        this.connectNodes(assetNode1, topologyNode1);
        this.addOutgoingTopologyRelationship(topologyNode1);

        AssetNode assetNode2 = this.createAssetNode("rail");
        TopologyNode topologyNode2 = this.createTopologyNode();
        this.connectNodes(assetNode2, topologyNode2);
        this.connectNodes(topologyNode1, topologyNode2);

        AssetNode assetNode3 = this.createAssetNode("kabel");
        TopologyNode topologyNode3 = this.createTopologyNode();
        this.connectNodes(assetNode3, topologyNode3);
        this.connectNodes(topologyNode2, topologyNode3);

        // First branch
        AssetNode assetNode4 = this.createAssetNode("transformator");
        TopologyNode topologyNode4 = this.createTopologyNode();
        this.connectNodes(assetNode4, topologyNode4);
        this.connectNodes(topologyNode3, topologyNode4);

        AssetNode assetNode5 = this.createAssetNode("transformator");
        TopologyNode topologyNode5 = this.createTopologyNode();
        this.connectNodes(assetNode5, topologyNode5);
        this.connectNodes(topologyNode4, topologyNode5);
        this.addOutgoingTopologyRelationship(topologyNode5);

        // Second branch
        AssetNode assetNode6 = this.createAssetNode("transformator");
        TopologyNode topologyNode6 = this.createTopologyNode();
        this.connectNodes(assetNode6, topologyNode6);
        this.connectNodes(topologyNode3, topologyNode6);

        AssetNode assetNode7 = this.createAssetNode("transformator");
        TopologyNode topologyNode7 = this.createTopologyNode();
        this.connectNodes(assetNode7, topologyNode7);
        this.connectNodes(topologyNode6, topologyNode7);
        this.addOutgoingTopologyRelationship(topologyNode7);
    }

    protected AssetNode createAssetNode(String label) {
        AssetNode node = new AssetNode(this.getNewId(), label);
        this.nodes.add(node);
        return node;
    }

    protected TopologyNode createTopologyNode() {
        TopologyNode node = new TopologyNode(this.getNewId());
        this.nodes.add(node);
        return node;
    }

    protected ContainerNode createContainerNode() {
        ContainerNode node = new ContainerNode(this.getNewId());
        this.nodes.add(node);
        return node;
    }

    protected void letContainerNodeAbsorbTopologyNode(ContainerNode containerNode, TopologyNode topologyNode) {
        for (Relationship relationship : topologyNode.relationships) {
            if (relationship.start == containerNode || relationship.end == containerNode) {
                // Relationships with containerNode can be removed completely.
                containerNode.relationships.remove(relationship);
                this.relationships.remove(relationship);
                continue;
            }

            if (relationship.start == topologyNode) {
                relationship.start = containerNode;
            } else {
                relationship.end = containerNode;
            }

            containerNode.relationships.add(relationship);
        }

        this.nodes.remove(topologyNode);
    }

    private Relationship connectNodes(Node start, Node end) {
        Relationship relationship = new Relationship(this.getNewId(), start, end);
        this.relationships.add(relationship);
        start.addRelationship(relationship);
        end.addRelationship(relationship);
        return relationship;
    }

    private OutgoingTopologyRelationship addOutgoingTopologyRelationship(TopologyNode node) {
        OutgoingTopologyRelationship relationship = new OutgoingTopologyRelationship(this.getNewId(), node);
        this.relationships.add(relationship);
        node.addRelationship(relationship);
        return relationship;
    }

    private long getNewId() {
        this.idCounter++;
        return this.idCounter;
    }

    protected void printNodes() {
        for (Node node : this.nodes) {
			System.out.println(String.format("%d - %s | %d R", node.id, node.label, node.relationships.size()));
			for (Relationship relationship : node.relationships) {
				Node otherNode = relationship.getOtherNode(node);
				if (otherNode == null) {
					System.out.println(String.format("\t\t\t<->%s", "<OUTGOING>"));						
				} else {
					System.out.println(String.format("\t\t\t<->%s", otherNode.id));						
				}
			}
		}
    }
}
