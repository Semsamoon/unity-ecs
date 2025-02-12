namespace ECS
{
    public readonly struct OptionsEntities
    {
        public const int DefaultCapacity = 64;
        public const int DefaultComponentsCapacity = 8;

        public readonly int Capacity;
        public readonly int ComponentsCapacity;

        public static OptionsEntities Default => new(DefaultCapacity, DefaultComponentsCapacity);

        public OptionsEntities(int capacity = DefaultCapacity, int componentsCapacity = DefaultComponentsCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Verifier.ArgumentWarning(nameof(componentsCapacity), componentsCapacity > 0, "should be greater than 0.");
            Capacity = capacity;
            ComponentsCapacity = componentsCapacity;
        }
    }
}