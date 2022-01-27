package alliandercrtest.connectivityregistry;

import java.util.ArrayList;
import java.util.List;

public class Node {
    public long id;
    public String label;

    protected List<Relationship> relationships = new ArrayList<>();

    public void addRelationship(Relationship relationship) {
        this.relationships.add(relationship);
    }
}
