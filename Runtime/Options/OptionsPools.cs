namespace ECS
{
    public readonly struct OptionsPools
    {
        public const int DefaultCapacity = 32;

        public readonly int Capacity;

        public OptionsPools(int capacity)
        {
            Capacity = capacity;
        }

        public static OptionsPools Default()
        {
            return new OptionsPools(DefaultCapacity);
        }

        public OptionsPools Validate()
        {
            return new OptionsPools(Capacity > 0 ? Capacity : DefaultCapacity);
        }
    }
}