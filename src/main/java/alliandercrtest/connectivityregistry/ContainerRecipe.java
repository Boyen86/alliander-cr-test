package alliandercrtest.connectivityregistry;

import java.util.ArrayList;
import java.util.List;

public class ContainerRecipe {
    List<Node> visited = new ArrayList<>();

    public ConnectivityGraph densify(ConnectivityGraph input) {
        // Not a big fan of returning the same object instead of creating a new one. 
        // Dependent on the context this can be OK or needs to be changed.

        List<Node> nodesCopy = new ArrayList<Node>();
        nodesCopy.addAll(input.nodes);
        for (Node node : nodesCopy) {
            if (this.visited.contains(node)) {
                continue;
            }

            this.visited.add(node);
            
            if (node instanceof TopologyNode) {
                TopologyNode topologyNode = (TopologyNode) node;
                List<TopologyNode> connectedNodes = this.findAllConnectedTopologyNodes(topologyNode);
                ContainerNode containerNode = input.createContainerNode();
                System.out.println(String.format("ContainerNode id: %d", containerNode.id));
                for (TopologyNode connectedNode : connectedNodes) {
                    input.letContainerNodeAbsorbTopologyNode(containerNode, connectedNode);
                }
            }
        }
        return input;
    }

    private List<TopologyNode> findAllConnectedTopologyNodes(TopologyNode node) {
        List<TopologyNode> connected = new ArrayList<>();
        connected.add(node);
        List<TopologyNode> toVisit = new ArrayList<>();
        toVisit.add(node);

        while (toVisit.size() > 0) {
            TopologyNode currentNode = toVisit.remove(0);
            this.visited.add(currentNode);

            for (Relationship relationship : currentNode.relationships) {
                if (relationship instanceof OutgoingTopologyRelationship) {
                    continue;
                }
    
                Node otherNode = relationship.getOtherNode(currentNode);
                if (!this.visited.contains(otherNode) && otherNode instanceof TopologyNode) {
                    TopologyNode otherTopologyNode = (TopologyNode) otherNode;
                    connected.add(otherTopologyNode);
                    toVisit.add(otherTopologyNode);
                }
            }
        }

        return connected;
    }
}
