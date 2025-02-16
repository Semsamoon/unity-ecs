namespace ECS
{
    public sealed class World : IWorld
    {
        public Entities Entities { get; }
        public Pools Pools { get; }
        public Filters Filters { get; }
        public Systems Systems { get; }

        IEntities IWorld.Entities => Entities;
        IPools IWorld.Pools => Pools;
        IFilters IWorld.Filters => Filters;
        ISystems IWorld.Systems => Systems;

        public static IWorld Create()
        {
            return new World();
        }

        public static IWorld Create(in Options options)
        {
            return new World(in options);
        }

        private World()
        {
            Entities = new Entities(this);
            Pools = new Pools(this);
            Filters = new Filters(this);
            Systems = new Systems(this);
        }

        private World(in Options options)
        {
            Entities = new Entities(this, options.EntitiesCapacity, options.EntityComponentsCapacity);
            Pools = new Pools(this,
                options.PoolsCapacity, options.EntitiesCapacity, options.PoolComponentsCapacity);
            Filters = new Filters(this,
                options.FiltersCapacity, options.FiltersWithSameComponentCapacity,
                options.EntitiesCapacity, options.FilterEntitiesCapacity);
            Systems = new Systems(this, options.SystemsCapacity);
        }

        public void Destroy()
        {
            Systems.Destroy();
        }
    }
}