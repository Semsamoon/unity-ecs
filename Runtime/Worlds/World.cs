namespace ECS
{
    public sealed class World : IWorld
    {
        public Entities EntitiesInternal { get; }
        public Pools PoolsInternal { get; }
        public Filters FiltersInternal { get; }
        public Systems SystemsInternal { get; }

        public IEntities Entities => EntitiesInternal;
        public IPools Pools => PoolsInternal;
        public IFilters Filters => FiltersInternal;
        public ISystems Systems => SystemsInternal;

        public World()
        {
            EntitiesInternal = new Entities(this);
            PoolsInternal = new Pools(this);
            FiltersInternal = new Filters(this);
            SystemsInternal = new Systems(this);
        }

        public World(OptionsEntities optionsEntities, int poolsCapacity, (int included, int excluded) filtersCapacity, int systemsCapacity)
        {
            EntitiesInternal = new Entities(this, optionsEntities);
            PoolsInternal = new Pools(this, poolsCapacity);
            FiltersInternal = new Filters(this, filtersCapacity.included, filtersCapacity.excluded);
            SystemsInternal = new Systems(this, systemsCapacity);
        }

        public void Destroy()
        {
            SystemsInternal.Destroy();
        }
    }
}