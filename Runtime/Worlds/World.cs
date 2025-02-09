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

        public World(OptionsEntities optionsEntities, OptionsPools optionsPools, OptionsPool optionsPool, OptionsFilters optionsFilters, OptionsFilter optionsFilter, OptionsSystems optionsSystems)
        {
            EntitiesInternal = new Entities(this, optionsEntities);
            PoolsInternal = new Pools(this, optionsPools, optionsPool);
            FiltersInternal = new Filters(this, optionsFilters, optionsFilter);
            SystemsInternal = new Systems(this, optionsSystems);
        }

        public void Destroy()
        {
            SystemsInternal.Destroy();
        }
    }
}