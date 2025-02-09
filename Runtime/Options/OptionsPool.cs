namespace ECS
{
    public readonly struct OptionsPool
    {
        public const int DefaultEntitiesCapacity = OptionsEntities.DefaultCapacity;
        public const int DefaultCapacity = 16;

        public readonly int EntitiesCapacity;
        public readonly int Capacity;

        public OptionsPool(int entitiesCapacity, int capacity)
        {
            EntitiesCapacity = entitiesCapacity;
            Capacity = capacity;
        }

        public static OptionsPool Default()
        {
            return new OptionsPool(DefaultEntitiesCapacity, DefaultCapacity);
        }

        public OptionsPool Validate()
        {
            return new OptionsPool(
                EntitiesCapacity > 1 ? EntitiesCapacity : DefaultEntitiesCapacity,
                Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}