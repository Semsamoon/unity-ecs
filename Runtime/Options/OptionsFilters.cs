namespace ECS
{
    public readonly struct OptionsFilters
    {
        public const int DefaultCapacity = 64;
        public const int DefaultFiltersCapacity = 8;

        public readonly int Capacity;
        public readonly int FiltersCapacity;

        public static OptionsFilters Default => new(DefaultCapacity, DefaultFiltersCapacity);

        public OptionsFilters(int capacity = DefaultCapacity, int filtersCapacity = DefaultFiltersCapacity)
        {
            Verifier.ArgumentWarning(nameof(capacity), capacity > 0, "should be greater than 0.");
            Verifier.ArgumentWarning(nameof(filtersCapacity), filtersCapacity > 0, "should be greater than 0.");
            Capacity = capacity;
            FiltersCapacity = filtersCapacity;
        }
    }
}