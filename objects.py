from dataclasses import dataclass
from enum import Enum
from typing import list

# Assignment 2
def MergedView(matchLabel, startNode) -> Node:
    """Transforms the (sub)graph view by merging adjacent nodes with similar
       assets."""

    def asset_filter(node):
        return [asset for asset in node.getAssets() if asset.NodeType == matchLabel]

    discovered_nodes = []
    undiscovered_nodes = startNode

    mergedNode = Node(NodeType.CONTAINER, [])

    for node in undiscovered_nodes:
        discovered_nodes.append(node)

        mergedNode.relations += substituteNode(node.OutgoingNodes, node, mergedNode)

        for n in node.TopologyNodeIterator:
            # This will lead to scaling issues (O(n^2)) on large graphs
            if n not in discovered_nodes:
                if node_asset_match(n):
                    undiscovered_nodes.append(n)

        mergedNode.relations += substituteNode(node.GetAssetRelations(), node, mergedNode)

    return mergedNode


def substituteNode(relations, old, new):
    """Substitute all occurences of node `old` with node `new` in the list of
       relations."""

    for r in relations:  # assume references
        if r.Node1 == old:
            r.Node1 = new

        if r.Node2 == old:
            r.Node2 = new

    return relations


def updateRelations(node: Node):
    """Update the ids of the relation of this node if it does not match, and the
       relation of the nodes it is related to"""
    # dummy
    pass


# Assignment 3
def SplitView(self, matchLabel, startNode) -> list(Node):
    """Transforms node view by merging adjacent nodes with similar assets and
       marks nodes with more than two topology relations."""

    undiscovered_nodes = [startNode]   # should maybe be a queue instead? no time to check
    discovered_nodes = []

    for node in undiscovered_nodes:
        # Change type and use this node in the graph
        if node.IsSplit():
            node = node.CopyAndChangeType(NodeType.SPLIT)
            updateRelations(node)
            discovered_nodes.append(node)
        else:
            # possibly returns its own argument
            node = MergedView(matchLabel, node)

            # Via the relations on the merged node we know the counterpart nodes,
            # so we can update their relations to point to the new nodes.
            updateRelations(node)
            discovered_nodes.append(node)

        # get the relations of either the split of the containerized node
        for n in node.TopologyNodeIterator:
            # This will lead to scaling issues (O(n^2)) on large graphs, there are smarter ways (e.g. search tree, but no time for that)
            if n not in discovered_nodes:
                undiscovered_nodes.append(n)

    # assert post-conditions
    assert(not undiscovered_nodes)
    assert(len(discovered_nodes) > 0)

    return discovered_nodes


class NodeType(Enum):
    TOPOLOGY = 0
    FIELD = 1
    RAIL = 2
    CABLE = 3
    TRANSFORMER = 4
    CONTAINER = 5
    SPLIT = 6

class RelationshipType(Enum):
    CONNECTS_TOPOLOGY = 0
    DESCRIBES_ASSET = 1
    OUTGOING = 2

@dataclass
class Relationship:
    id_: long       # use ctypes for binary compatibility?
    type_: RelationshipType
    label: str
    node1: Node
    node2: Node

def getConnectingNode(relation: Relationship, node: Node) -> Node:
    """Find the other node in a relation"""

    if relation.type_ is RelationshipType.OUTGOING:
        raise TypeError("Outgoing relations have no corresponding node")

    return relation.node1 if node.id_ == relation.node2.id_ else relation.node1


class Node:
    id_: long
    relations: list[Relationships]
    type_: NodeType

    def __init__(self, nodetype, relations, id_=None):
        if id_ is None:
            self.id_ = set_random_id()
        self.type_ = nodetype
        self.relations = relations

    def getAssetRelations(self) -> list(Node):
        if self.type_ != NodeType.TOPOLOGY:
            raise ValueError("Only a topo node can have an asset")

        return filter(lambda relation: node.type_ == RelationshipType.DESCRIBES_ASSET, self.relations)

    def isSplit(self) -> bool:
        return len(filter(lambda r: r.type_ == RelationshipType.CONNECTS_TOPOLOGY, self.relations)) >= 3

    def isAsset(self) -> bool:
        return self.type_ is NodeType.FIELD \
            or self.type_ is NodeType.RAIL \
            or self.type_ is NodeType.CABLE \
            or self.type_ is NodeType.TRANSFORMER

    def isTopology(self: NodeType) -> bool:
        return self.type_ is NodeType.TOPOLOGY

    # Iterator over all the topology nodes that are connected
    def TopologyNodeIterator(self):
        for r in self.relations:
            if r.type_ == RelationshipType.CONNECTS_TOPOLOGY:
                yield r.getConnectingNode(self)
