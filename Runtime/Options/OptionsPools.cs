namespace ECS
{
    public readonly struct OptionsPools
    {
        public const int DefaultCapacity = 32;

        public readonly int Capacity;

        public static OptionsPools Default => new(DefaultCapacity);

        public OptionsPools(int capacity = DefaultCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Capacity = capacity;
        }
    }
}