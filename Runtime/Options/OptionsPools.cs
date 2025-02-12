namespace ECS
{
    public readonly struct OptionsPools
    {
        public const int DefaultCapacity = 32;

        public readonly int Capacity;

        public static OptionsPools Default => new(DefaultCapacity);

        public OptionsPools(int capacity)
        {
            Capacity = capacity;
        }

        public OptionsPools Validate()
        {
            return new OptionsPools(Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}