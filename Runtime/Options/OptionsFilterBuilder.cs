namespace ECS
{
    public readonly struct OptionsFilterBuilder
    {
        public const int DefaultIncludedCapacity = 4;
        public const int DefaultExcludedCapacity = 4;

        public readonly int IncludedCapacity;
        public readonly int ExcludedCapacity;

        public OptionsFilterBuilder(int includedCapacity, int excludedCapacity)
        {
            IncludedCapacity = includedCapacity;
            ExcludedCapacity = excludedCapacity;
        }

        public static OptionsFilterBuilder Default()
        {
            return new OptionsFilterBuilder(DefaultIncludedCapacity, DefaultExcludedCapacity);
        }

        public OptionsFilterBuilder Validate()
        {
            return new OptionsFilterBuilder(
                IncludedCapacity > 0 ? IncludedCapacity : DefaultIncludedCapacity,
                ExcludedCapacity > 0 ? ExcludedCapacity : DefaultExcludedCapacity);
        }
    }
}