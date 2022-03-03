using Alliander.Connectivity.Registry.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Alliander.Connectivity.Registry
{
    public class Relationship :IRelationship
    {
        public Relationship()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(5);

        public void CreateRelationship(long from, long to, INodes node)
        {
            var nodeFrom = node.GetNode(from);
            var nodeTo = node.GetNode(to);
            if (nodeFrom != null && nodeTo != null)
            {
                var label = DetermineLabel(nodeFrom, nodeTo);
                var relationship = SaveRelationship(nodeFrom, nodeTo, label);
                node.UpdateNodes(nodeFrom, nodeTo, relationship);
            }
        }

        private Relationships SaveRelationship(Node nodeFrom, Node nodeTo, string label)
        {
            var relationship = new Relationships() { Id = 0, Label = label, NodeFrom = nodeFrom, NodeTo = nodeTo };
            if (!_cache.TryGetValue("relationships", out List<Relationships> outputRelationships))
            {
                outputRelationships = new List<Relationships>();
            }
            else
            {
                var id = outputRelationships.Max(x => x.Id);
                relationship.Id = Interlocked.Increment(ref id);
            }
            outputRelationships.Add(relationship);
            _cache.Set("relationships", outputRelationships, _cacheTimeout);
            return relationship;
        }

        private string DetermineLabel(Node nodeFrom, Node nodeTo)
        {
            if(nodeFrom.Label == "topology" && nodeTo.Label == "topology")
            {
                return "CONNECTS_TOPOLOGY";
            }
            return "DESCRIBES_ASSET";
        }
    }
}
