package alliandercrtest.connectivityregistry;

import java.util.Arrays;
import java.util.List;

public class AssetNode extends Node {

    private static List<String> allowedLabels = Arrays.asList("veld", "rail", "kabel", "transformator");
    
    public AssetNode(long id, String label) {
        if (!allowedLabels.contains(label)) {
            // I'm not happy that the class name is hardcoded here.
            throw new IllegalArgumentException(String.format("'%s' is not a valid AssetNode label", label));
        }
        this.id = id;
        this.label = label;
    }

}
