namespace ECS
{
    public struct OptionsWorld
    {
        public OptionsEntities Entities { get; private set; }
        public OptionsPools Pools { get; private set; }
        public OptionsPool Pool { get; private set; }
        public OptionsFilters Filters { get; private set; }
        public OptionsFilter Filter { get; private set; }
        public OptionsSystems Systems { get; private set; }

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