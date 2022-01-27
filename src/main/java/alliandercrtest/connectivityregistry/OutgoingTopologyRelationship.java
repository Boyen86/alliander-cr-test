package alliandercrtest.connectivityregistry;

public class OutgoingTopologyRelationship extends Relationship {
    public OutgoingTopologyRelationship(long id, Node start) {
        super(id, start, null, "CONNECTS_TOPLOGY");
    }
}
