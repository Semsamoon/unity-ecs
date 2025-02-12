namespace ECS
{
    public struct OptionsWorld
    {
        public OptionsEntities Entities;
        public OptionsPools Pools;
        public OptionsPool Pool;
        public OptionsFilters Filters;
        public OptionsFilter Filter;
        public OptionsSystems Systems;

        public static OptionsWorld Default => new OptionsWorld()
            .With(OptionsEntities.Default)
            .With(OptionsPools.Default)
            .With(OptionsPool.Default)
            .With(OptionsFilters.Default)
            .With(OptionsFilter.Default)
            .With(OptionsSystems.Default);

        public OptionsWorld With(OptionsEntities entities)
        {
            Entities = entities;
            return this;
        }

        public OptionsWorld With(OptionsPools pools)
        {
            Pools = pools;
            return this;
        }

        public OptionsWorld With(OptionsPool pool)
        {
            Pool = pool;
            return this;
        }

        public OptionsWorld With(OptionsFilters filters)
        {
            Filters = filters;
            return this;
        }

        public OptionsWorld With(OptionsFilter filter)
        {
            Filter = filter;
            return this;
        }

        public OptionsWorld With(OptionsSystems systems)
        {
            Systems = systems;
            return this;
        }
    }
}