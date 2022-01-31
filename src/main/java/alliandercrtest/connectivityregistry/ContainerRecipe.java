package alliandercrtest.connectivityregistry;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class ContainerRecipe {
    Set<Node> visited = new HashSet<>();
    private NodeFactory nodeFactory;

    public ContainerRecipe(NodeFactory nodeFactory) {
        this.nodeFactory = nodeFactory;
    }
    public ConnectivityGraph densify(ConnectivityGraph input) {
        // Not a big fan of returning the same object instead of creating a new one. 
        // Dependent on the context this can be OK or needs to be changed.
        // At the same time, the old graph basically becomes corrupt if we change
        // the relationships.. or, we should not change the relationships, just create new ones
        // for the new graph.. No that's also not possible, the relationship ID's should stay the same.
        // So we'd have to first do a deep clone of the entire graph + nodes & relationships before we perform
        // this function. This probably tells us the current data-structure might not be perfectly suited for
        // what we want to do.
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
                ContainerNode containerNode = nodeFactory.createContainerNode();
                input.registerNode(containerNode);
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
