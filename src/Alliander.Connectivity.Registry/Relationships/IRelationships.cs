namespace Alliander.Connectivity.Registry
{
    public interface IRelationship
    {
        /// <summary>
        /// Create a relationship
        /// </summary>
        /// <param name="from">Start node <see cref="Nodes"/> of the relationship.</param>
        /// <param name="to">End node <see cref="Nodes"/> of the relationship.</param>
        /// <param name="node">Instance of <see cref="INodes"/></param>
        public void CreateRelationship(long from, long to, INodes node);
    }
}
