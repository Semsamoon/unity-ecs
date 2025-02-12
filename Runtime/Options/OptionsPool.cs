namespace ECS
{
    public readonly struct OptionsPool
    {
        public const int DefaultCapacity = 16;

        public readonly int Capacity;

        public static OptionsPool Default => new(DefaultCapacity);

        public OptionsPool(int capacity = DefaultCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Capacity = capacity;
        }
    }
}