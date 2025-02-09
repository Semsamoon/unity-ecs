namespace ECS
{
    public readonly struct OptionsFilter
    {
        public const int DefaultEntitiesCapacity = OptionsEntities.DefaultCapacity;
        public const int DefaultCapacity = 16;

        public readonly int EntitiesCapacity;
        public readonly int Capacity;

        public OptionsFilter(int entitiesCapacity, int capacity)
        {
            EntitiesCapacity = entitiesCapacity;
            Capacity = capacity;
        }

        public static OptionsFilter Default()
        {
            return new OptionsFilter(DefaultEntitiesCapacity, DefaultCapacity);
        }

        public OptionsFilter Validate()
        {
            return new OptionsFilter(
                EntitiesCapacity > 1 ? EntitiesCapacity : DefaultEntitiesCapacity,
                Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}