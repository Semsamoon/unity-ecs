namespace ECS
{
    public readonly struct OptionsFilter
    {
        public const int DefaultEntitiesCapacity = OptionsEntities.DefaultCapacity;
        public const int DefaultCapacity = 16;

        public readonly int EntitiesCapacity;
        public readonly int Capacity;

        public static OptionsFilter Default => new(DefaultEntitiesCapacity, DefaultCapacity);

        public OptionsFilter(int entitiesCapacity, int capacity)
        {
            EntitiesCapacity = entitiesCapacity;
            Capacity = capacity;
        }

        public OptionsFilter Validate()
        {
            return new OptionsFilter(
                EntitiesCapacity > 1 ? EntitiesCapacity : DefaultEntitiesCapacity,
                Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}