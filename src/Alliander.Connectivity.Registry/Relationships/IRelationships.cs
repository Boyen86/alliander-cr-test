namespace Alliander.Connectivity.Registry
{
    public interface IRelationship
    {
        public void CreateRelationship(long from, long to, INodes node);
    }
}
