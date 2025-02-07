namespace ECS
{
    public sealed class World : IWorld
    {
        public Entities EntitiesInternal { get; }
        public Pools PoolsInternal { get; }
        public Filters FiltersInternal { get; }

        public IEntities Entities => EntitiesInternal;
        public IPools Pools => PoolsInternal;
        public IFilters Filters => FiltersInternal;

        public World()
        {
            EntitiesInternal = new Entities();
            PoolsInternal = new Pools();
            FiltersInternal = new Filters(this);
        }

        public World((int sparse, int dense) entitiesCapacity, int poolsCapacity, (int included, int excluded) filtersCapacity)
        {
            EntitiesInternal = new Entities(entitiesCapacity.sparse, entitiesCapacity.dense);
            PoolsInternal = new Pools(poolsCapacity);
            FiltersInternal = new Filters(this, filtersCapacity.included, filtersCapacity.excluded);
        }
    }
}