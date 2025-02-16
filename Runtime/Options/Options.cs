namespace ECS
{
    /// <summary>
    /// Represents a collection of options used during the creation of the <see cref="World"/>.
    /// </summary>
    public readonly struct Options
    {
        public const int DefaultEntitiesCapacity = 128;
        public const int DefaultEntityComponentsCapacity = 8;
        public const int DefaultPoolsCapacity = 64;
        public const int DefaultPoolComponentsCapacity = 32;
        public const int DefaultFiltersCapacity = 64;
        public const int DefaultFiltersWithSameComponentCapacity = 8;
        public const int DefaultFilterEntitiesCapacity = 32;
        public const int DefaultSystemsCapacity = 64;

        public readonly int EntitiesCapacity;
        public readonly int EntityComponentsCapacity;
        public readonly int PoolsCapacity;
        public readonly int PoolComponentsCapacity;
        public readonly int FiltersCapacity;
        public readonly int FiltersWithSameComponentCapacity;
        public readonly int FilterEntitiesCapacity;
        public readonly int SystemsCapacity;

        public static Options Default => new(entitiesCapacity: DefaultEntitiesCapacity);

        public Options(
            int entitiesCapacity = DefaultEntitiesCapacity, int entityComponentsCapacity = DefaultEntityComponentsCapacity,
            int poolsCapacity = DefaultPoolsCapacity, int poolComponentsCapacity = DefaultPoolComponentsCapacity,
            int filtersCapacity = DefaultFiltersCapacity, int filtersWithSameComponentCapacity = DefaultFiltersWithSameComponentCapacity,
            int filterEntitiesCapacity = DefaultFilterEntitiesCapacity, int systemsCapacity = DefaultSystemsCapacity)
        {
            Verifier.ArgumentWarning(nameof(entitiesCapacity), entitiesCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(entityComponentsCapacity), entityComponentsCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(poolsCapacity), poolsCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(poolComponentsCapacity), poolComponentsCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(filtersCapacity), filtersCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(filtersWithSameComponentCapacity), filtersWithSameComponentCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(filterEntitiesCapacity), filterEntitiesCapacity > 0, "must be greater than 0");
            Verifier.ArgumentWarning(nameof(systemsCapacity), systemsCapacity > 0, "must be greater than 0");

            EntitiesCapacity = entitiesCapacity;
            EntityComponentsCapacity = entityComponentsCapacity;
            PoolsCapacity = poolsCapacity;
            PoolComponentsCapacity = poolComponentsCapacity;
            FiltersCapacity = filtersCapacity;
            FiltersWithSameComponentCapacity = filtersWithSameComponentCapacity;
            FilterEntitiesCapacity = filterEntitiesCapacity;
            SystemsCapacity = systemsCapacity;
        }
    }
}