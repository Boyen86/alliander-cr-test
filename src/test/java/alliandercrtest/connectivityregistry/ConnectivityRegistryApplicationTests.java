package alliandercrtest.connectivityregistry;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
class ConnectivityRegistryApplicationTests {

	@Test
	void contextLoads() {
	}

	@Test
	void wrongLabelAssetNode() throws Exception {
		assertThrows(IllegalArgumentException.class, () -> new AssetNode(1, "bad_label"));
	}

	@Test
	void testDensifyRecipeSimple() {
		ConnectivityGraph connectivityGraph = new ConnectivityGraph();
		connectivityGraph.initializeSimpleGraph();

		ContainerRecipe containerRecipe = new ContainerRecipe();
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(5, result.nodes.size());
		ContainerNode cNode = (ContainerNode) result.nodes.get(4);
		assertEquals(6, cNode.relationships.size());
	}

	@Test
	void testDensifyRecipeComplex() {
		ConnectivityGraph connectivityGraph = new ConnectivityGraph();
		connectivityGraph.initializeComplexGraph();

		ContainerRecipe containerRecipe = new ContainerRecipe();
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(8, result.nodes.size());
		ContainerNode cNode = (ContainerNode) result.nodes.get(7);
		assertEquals(10, cNode.relationships.size());
	}

	@Test
	void testDensifySplitRecipeSimple() {
		ConnectivityGraph connectivityGraph = new ConnectivityGraph();
		connectivityGraph.initializeSimpleGraph();

		SplitContainerRecipe containerRecipe = new SplitContainerRecipe();
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(5, result.nodes.size());
		ContainerNode cNode = (ContainerNode) result.nodes.get(4);
		assertEquals(6, cNode.relationships.size());
	}

	@Test
	void testDensifySplitRecipeComplex() {
		ConnectivityGraph connectivityGraph = new ConnectivityGraph();
		connectivityGraph.initializeComplexGraph();
		connectivityGraph.printNodes();

		SplitContainerRecipe containerRecipe = new SplitContainerRecipe();
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(11, result.nodes.size());
	}
}
