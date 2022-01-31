package alliandercrtest.connectivityregistry;

public class RelationshipFactory {
    private long idCounter = 0;

    private long getNewId() {
        this.idCounter++;
        return this.idCounter;
    }

    protected Relationship createRelationship(Node start, Node end) {
        Relationship relationship = new Relationship(this.getNewId(), start, end);
        start.addRelationship(relationship);
        end.addRelationship(relationship);
        return relationship;
    }

    protected OutgoingTopologyRelationship createOutgoingTopologyRelationship(TopologyNode node) {
        OutgoingTopologyRelationship relationship = new OutgoingTopologyRelationship(this.getNewId(), node);
        node.addRelationship(relationship);
        return relationship;
    }

}
