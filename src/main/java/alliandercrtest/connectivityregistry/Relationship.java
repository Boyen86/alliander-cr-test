package alliandercrtest.connectivityregistry;

public class Relationship {
    public long id;
    public String label;

    // Bit misleading name, since the relationship is _not_ directed.
    // 
    public Node start; 
    public Node end;

    public Relationship(long id, Node start, Node end) {
        this.id = id;
        this.start = start;
        this.end = end;
        this.label = this.determineLabel(start, end);
    }

    public Relationship(long id, Node start, Node end, String label) {
        this.id = id;
        this.start = start;
        this.end = end;
        this.label = label;
    }

    private String determineLabel(Node start, Node end) {
        if (start instanceof TopologyNode && end instanceof TopologyNode) {
            return "CONNECTS_TOPLOGY"; // Shouldn't it be TOPOLOGY?
        } else if ((start instanceof TopologyNode && end instanceof AssetNode )||
            (start instanceof AssetNode && end instanceof TopologyNode)) {
            return "DESCRIBES_ASSET";
        } else {
            throw new IllegalArgumentException(); // Change to custom exception
        }
    }

    public Node getOtherNode(Node node) {
        if (node != this.start && node != this.end) {
            throw new IllegalArgumentException(String.format("Node with id '%d' is not part of the relationship", node.id));
        }

        if (node == this.start) {
            return this.end;
        } else {
            return this.start;
        }
    }
}
