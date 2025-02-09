namespace ECS
{
    public readonly struct OptionsFilters
    {
        public const int DefaultCapacity = 32;
        public const int DefaultFiltersCapacity = 8;

        public readonly int Capacity;
        public readonly int FiltersCapacity;

        public OptionsFilters(int capacity, int filtersCapacity)
        {
            Capacity = capacity;
            FiltersCapacity = filtersCapacity;
        }

        public static OptionsFilters Default()
        {
            return new OptionsFilters(DefaultCapacity, DefaultFiltersCapacity);
        }

        public OptionsFilters Validate()
        {
            return new OptionsFilters(
                Capacity > 0 ? Capacity : DefaultCapacity,
                FiltersCapacity > 0 ? FiltersCapacity : DefaultFiltersCapacity);
        }
    }
}