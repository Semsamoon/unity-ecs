namespace ECS
{
    public readonly struct OptionsSystems
    {
        public const int DefaultCapacity = 16;

        public readonly int Capacity;

        public OptionsSystems(int capacity)
        {
            Capacity = capacity;
        }

        public static OptionsSystems Default()
        {
            return new OptionsSystems(DefaultCapacity);
        }

        public OptionsSystems Validate()
        {
            return new OptionsSystems(Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}