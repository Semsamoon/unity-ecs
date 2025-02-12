namespace ECS
{
    public readonly struct OptionsEntities
    {
        public const int DefaultCapacity = 64;
        public const int DefaultComponentsCapacity = 8;

        public readonly int Capacity;
        public readonly int ComponentsCapacity;

        public static OptionsEntities Default => new(DefaultCapacity, DefaultComponentsCapacity);

        public OptionsEntities(int capacity, int componentsCapacity)
        {
            Capacity = capacity;
            ComponentsCapacity = componentsCapacity;
        }

        public OptionsEntities Validate()
        {
            return new OptionsEntities(
                Capacity > 1 ? Capacity : DefaultCapacity,
                ComponentsCapacity > 0 ? ComponentsCapacity : DefaultComponentsCapacity);
        }
    }
}