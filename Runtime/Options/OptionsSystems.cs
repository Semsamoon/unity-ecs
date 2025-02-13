namespace ECS
{
    public readonly struct OptionsSystems
    {
        public const int DefaultCapacity = 64;

        public readonly int Capacity;

        public static OptionsSystems Default => new(DefaultCapacity);

        public OptionsSystems(int capacity = DefaultCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Capacity = capacity;
        }
    }
}