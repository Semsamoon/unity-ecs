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

        public static IWorld Create()
        {
            return new World(OptionsWorld.Default);
        }

        public static IWorld Create(OptionsWorld world)
        {
            return new World(world);
        }

        private World(OptionsWorld options)
        {
            EntitiesInternal = new Entities(this, in options.Entities);
            PoolsInternal = new Pools(this, in options.Pools, in options.Pool, in options.Entities);
            FiltersInternal = new Filters(this, in options.Filters, in options.Filter, in options.Entities);
            SystemsInternal = new Systems(this, in options.Systems);
        }

        public void Destroy()
        {
            SystemsInternal.Destroy();
        }
    }
}