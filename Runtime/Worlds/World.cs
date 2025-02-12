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

        public World(OptionsWorld options)
        {
            EntitiesInternal = new Entities(this, options.Entities);
            PoolsInternal = new Pools(this, options.Pools, options.Pool, options.Entities);
            FiltersInternal = new Filters(this, options.Filters, options.Filter, options.Entities);
            SystemsInternal = new Systems(this, options.Systems);
        }

        public void Destroy()
        {
            SystemsInternal.Destroy();
        }
    }
}