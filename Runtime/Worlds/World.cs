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
            return new World(OptionsWorld.Default);
        }

        public static IWorld Create(OptionsWorld world)
        {
            return new World(world);
        }

        private World(OptionsWorld options)
        {
            Entities = new Entities(this, in options.Entities);
            Pools = new Pools(this, in options.Pools, in options.Pool, in options.Entities);
            Filters = new Filters(this, in options.Filters, in options.Filter, in options.Entities);
            Systems = new Systems(this, in options.Systems);
        }

        public void Destroy()
        {
            Systems.Destroy();
        }
    }
}