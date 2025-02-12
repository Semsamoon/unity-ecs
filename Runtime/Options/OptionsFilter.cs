namespace ECS
{
    public readonly struct OptionsFilter
    {
        public const int DefaultCapacity = 16;

        public readonly int Capacity;

        public static OptionsFilter Default => new(DefaultCapacity);

        public OptionsFilter(int capacity = DefaultCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Capacity = capacity;
        }
    }
}