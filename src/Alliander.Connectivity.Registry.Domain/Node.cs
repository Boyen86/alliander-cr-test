using System;
using System.Collections.Generic;

namespace Alliander.Connectivity.Registry.Domain
{
    /// <summary>
    /// Respresentaion of a Node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Identifier of the Node
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// List of relationships assigned to the node
        /// </summary>
        public IEnumerable<Relationships> Relationship { get; set; }

        /// <summary>
        /// Label of the Node
        /// </summary>
        public string Label { get; set; }

    }
}
