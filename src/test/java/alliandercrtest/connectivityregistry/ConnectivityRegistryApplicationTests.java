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
		NodeFactory nodeFactory = new NodeFactory();
		RelationshipFactory relationshipFactory = new RelationshipFactory();
		GraphFactory graphFactory = new GraphFactory(nodeFactory, relationshipFactory);
		ConnectivityGraph connectivityGraph = graphFactory.createSimpleGraph();

		ContainerRecipe containerRecipe = new ContainerRecipe(nodeFactory);
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(5, result.nodes.size());

		ContainerNode cNode = (ContainerNode) result.nodes.get(4);
		assertEquals(6, cNode.relationships.size());
	}

	@Test
	void testDensifyRecipeComplex() {
		NodeFactory nodeFactory = new NodeFactory();
		RelationshipFactory relationshipFactory = new RelationshipFactory();
		GraphFactory graphFactory = new GraphFactory(nodeFactory, relationshipFactory);
		ConnectivityGraph connectivityGraph = graphFactory.createComplexGraph();

		ContainerRecipe containerRecipe = new ContainerRecipe(nodeFactory);
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(8, result.nodes.size());
		ContainerNode cNode = (ContainerNode) result.nodes.get(7);
		assertEquals(10, cNode.relationships.size());
	}

	@Test
	void testDensifySplitRecipeSimple() {
		NodeFactory nodeFactory = new NodeFactory();
		RelationshipFactory relationshipFactory = new RelationshipFactory();
		GraphFactory graphFactory = new GraphFactory(nodeFactory, relationshipFactory);
		ConnectivityGraph connectivityGraph = graphFactory.createSimpleGraph();

		SplitContainerRecipe containerRecipe = new SplitContainerRecipe(nodeFactory);
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(5, result.nodes.size());
		ContainerNode cNode = (ContainerNode) result.nodes.get(4);
		assertEquals(6, cNode.relationships.size());
	}

	@Test
	void testDensifySplitRecipeComplex() {
		NodeFactory nodeFactory = new NodeFactory();
		RelationshipFactory relationshipFactory = new RelationshipFactory();
		GraphFactory graphFactory = new GraphFactory(nodeFactory, relationshipFactory);
		ConnectivityGraph connectivityGraph = graphFactory.createComplexGraph();

		SplitContainerRecipe containerRecipe = new SplitContainerRecipe(nodeFactory);
		ConnectivityGraph result = containerRecipe.densify(connectivityGraph);
		result.printNodes();

		assertEquals(11, result.nodes.size());
	}

}
