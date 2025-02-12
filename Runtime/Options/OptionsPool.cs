namespace ECS
{
    public readonly struct OptionsPool
    {
        public const int DefaultEntitiesCapacity = OptionsEntities.DefaultCapacity;
        public const int DefaultCapacity = 16;

        public readonly int EntitiesCapacity;
        public readonly int Capacity;

        public static OptionsPool Default => new(DefaultEntitiesCapacity, DefaultCapacity);

        public OptionsPool(int entitiesCapacity = DefaultEntitiesCapacity, int capacity = DefaultCapacity)
        {
            Verifier.ArgumentWarning(nameof(entitiesCapacity), entitiesCapacity > 0, "should be greater than 0.");
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            EntitiesCapacity = entitiesCapacity;
            Capacity = capacity;
        }
    }
}