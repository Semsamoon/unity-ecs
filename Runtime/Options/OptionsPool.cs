namespace ECS
{
    public readonly struct OptionsPool
    {
        public const int DefaultEntitiesCapacity = OptionsEntities.DefaultCapacity;
        public const int DefaultCapacity = 16;

        public readonly int EntitiesCapacity;
        public readonly int Capacity;

        public static OptionsPool Default => new(DefaultEntitiesCapacity, DefaultCapacity);

        public OptionsPool(int entitiesCapacity, int capacity)
        {
            EntitiesCapacity = entitiesCapacity;
            Capacity = capacity;
        }

        public OptionsPool Validate()
        {
            return new OptionsPool(
                EntitiesCapacity > 1 ? EntitiesCapacity : DefaultEntitiesCapacity,
                Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}