package alliandercrtest.connectivityregistry;

import java.util.ArrayList;
import java.util.List;

public class ConnectivityGraph {
    // What is meant with the requirement "given a layer name you can easily find data on that layer"?
    
    protected List<Node> nodes = new ArrayList<>();
    protected List<Relationship> relationships = new ArrayList<>();


    public void registerNode(Node node) {
        this.nodes.add(node);
    }

    public void registerRelationship(Relationship relationship) {
        this.relationships.add(relationship);
    }

    protected void letContainerNodeAbsorbTopologyNode(ContainerNode containerNode, TopologyNode topologyNode) {
        // I left this here instead of putting it in the ContainerNode class, because if I did that, the ContainerNode
        // class would have to have knowledge of the data-structure in which nodes and relationships are stored.
        // Of course, I guess an api on the graph of 'removeRelationship' and 'removeNode' is not that bad, but still.
        // I'd think the Node could be/stay stupid. That said, maybe it should not be part of this class but of the
        // recipe.. hm that also doesn't sound exactly right. I agree that it should not be in this class though.
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
