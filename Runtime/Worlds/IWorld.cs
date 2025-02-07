namespace ECS
{
    public interface IWorld
    {
        public IEntities Entities { get; }
        public IPools Pools { get; }
        public IFilters Filters { get; }
        public ISystems Systems { get; }
    }
}