package alliandercrtest.connectivityregistry;

public class NodeFactory {
    private long idCounter = 0;

    private long getNewId() {
        this.idCounter++;
        return this.idCounter;
    }

    protected AssetNode createAssetNode(String label) {
        return new AssetNode(this.getNewId(), label);
    }

    protected TopologyNode createTopologyNode() {
        return new TopologyNode(this.getNewId());
    }

    protected ContainerNode createContainerNode() {
        return new ContainerNode(this.getNewId());
    }
}
