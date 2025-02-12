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

        public OptionsWorld With(in OptionsEntities entities)
        {
            Entities = entities;
            return this;
        }

        public OptionsWorld With(in OptionsPools pools)
        {
            Pools = pools;
            return this;
        }

        public OptionsWorld With(in OptionsPool pool)
        {
            Pool = pool;
            return this;
        }

        public OptionsWorld With(in OptionsFilters filters)
        {
            Filters = filters;
            return this;
        }

        public OptionsWorld With(in OptionsFilter filter)
        {
            Filter = filter;
            return this;
        }

        public OptionsWorld With(in OptionsSystems systems)
        {
            Systems = systems;
            return this;
        }
    }
}