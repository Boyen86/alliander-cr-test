import unittest

from parameterized import parameterized

class TestNodeMethods(unittest.TestCase):

    def testRelations(self):
        pass

    def testNode(self):
        pass
        # The node class is fairly simple to unit test


    # Draw and solve some common scenarios on paper and check if the solution
    # matches. Can be used for assignment 2 and 3
    @parameterized.expand([
         [("graph1", Splitview, graph_test, graph_min) ...]
    ])
    def testSplitting(self, name, fcn, data_in, solution):
        res = fcn(data_in[0])

        self.AssertEqualArray(sort(getNodeSet(res)), sort(getNodeSet(solution)))
        self.AssertEqualArray(sort(getRelationsSet(res)), sort(getRelationsSet(solution)))



if __name__ == '__main__':
    unittest.main()
