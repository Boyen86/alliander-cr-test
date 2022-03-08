

namespace Alliander.Connectivity.Registry.Domain
{
    /// <summary>
    /// Respresentaion of a relationship
    /// </summary>
    public class Relationships
    {
        /// <summary>
        /// Identifier of the relationship
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Label of the relationship
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Connected Node of one side of the relationship.
        /// </summary>
        public Node NodeFrom { get; set; }

        /// <summary>
        /// Connected Node of other side of the relationship.
        /// </summary>
        public Node NodeTo { get; set; }

    }
}
