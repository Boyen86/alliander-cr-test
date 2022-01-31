package alliandercrtest.connectivityregistry;

import java.util.Set;

public class AssetNode extends Node {

    private final static Set<String> allowedLabels = Set.of("veld", "rail", "kabel", "transformator");
    
    public AssetNode(long id, String label) {
        if (!allowedLabels.contains(label)) {
            // I'm not happy that the class name is hardcoded here.
            throw new IllegalArgumentException(String.format("'%s' is not a valid AssetNode label", label));
        }
        this.id = id;
        this.label = label;
    }

}
