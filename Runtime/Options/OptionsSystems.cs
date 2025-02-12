namespace ECS
{
    public readonly struct OptionsSystems
    {
        public const int DefaultCapacity = 16;

        public readonly int Capacity;

        public static OptionsSystems Default => new(DefaultCapacity);

        public OptionsSystems(int capacity)
        {
            Capacity = capacity;
        }

        public OptionsSystems Validate()
        {
            return new OptionsSystems(Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}