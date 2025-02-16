using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Filters is a manager for filters.
    /// </summary>
    public sealed class Filters : IFilters
    {
        private readonly World _world;
        private readonly Dictionary<Type, DenseArray<Filter>> _included;
        private readonly Dictionary<Type, DenseArray<Filter>> _excluded;

        private readonly int _filtersWithSameComponentCapacity;
        private readonly int _entitiesCapacity;
        private readonly int _filterEntitiesCapacity;

        public (int included, int excluded) Length => (_included.Count, _excluded.Count);

        public Filters(
            World world,
            int filtersCapacity = Options.DefaultFiltersCapacity,
            int filtersWithSameComponentCapacity = Options.DefaultFiltersWithSameComponentCapacity,
            int entitiesCapacity = Options.DefaultEntitiesCapacity,
            int filterEntitiesCapacity = Options.DefaultFilterEntitiesCapacity)
        {
            _world = world;
            _included = new Dictionary<Type, DenseArray<Filter>>(filtersCapacity);
            _excluded = new Dictionary<Type, DenseArray<Filter>>(filtersCapacity);
            _filtersWithSameComponentCapacity = filtersWithSameComponentCapacity;
            _entitiesCapacity = entitiesCapacity;
            _filterEntitiesCapacity = filterEntitiesCapacity;
        }

        IFilterBuilderEmpty IFilters.Create()
        {
            return new FilterBuilder(_world, _entitiesCapacity, _filterEntitiesCapacity);
        }

        public FilterBuilder Create()
        {
            return new FilterBuilder(_world, _entitiesCapacity, _filterEntitiesCapacity);
        }

        IFilterBuilderEmpty IFilters.Create(int filterEntitiesCapacity)
        {
            return new FilterBuilder(_world, _entitiesCapacity, filterEntitiesCapacity);
        }

        public FilterBuilder Create(int filterEntitiesCapacity)
        {
            return new FilterBuilder(_world, _entitiesCapacity, filterEntitiesCapacity);
        }

        IFilters IFilters.IncludeCapacity<T>(int capacity)
        {
            EnsureCapacity(_included, typeof(T), capacity);
            return this;
        }

        public Filters IncludeCapacity<T>(int capacity)
        {
            EnsureCapacity(_included, typeof(T), capacity);
            return this;
        }

        IFilters IFilters.ExcludeCapacity<T>(int capacity)
        {
            EnsureCapacity(_excluded, typeof(T), capacity);
            return this;
        }

        public Filters ExcludeCapacity<T>(int capacity)
        {
            EnsureCapacity(_excluded, typeof(T), capacity);
            return this;
        }

        public Filters Include(Filter filter, Type type)
        {
            Add(_included, filter, type, _filtersWithSameComponentCapacity);
            return this;
        }

        public Filters Include(Filter filter, Type type, int capacity)
        {
            Add(_included, filter, type, capacity);
            return this;
        }

        public Filters Exclude(Filter filter, Type type)
        {
            Add(_excluded, filter, type, _filtersWithSameComponentCapacity);
            return this;
        }

        public Filters Exclude(Filter filter, Type type, int capacity)
        {
            Add(_excluded, filter, type, capacity);
            return this;
        }

        public Filters RecordUnchecked(Entity entity, Type type)
        {
            Change(_included, type, entity, 1);
            Change(_excluded, type, entity, -1);
            return this;
        }

        public Filters EraseUnchecked(Entity entity, Type type)
        {
            Change(_included, type, entity, -1);
            Change(_excluded, type, entity, 1);
            return this;
        }

        private static void EnsureCapacity(Dictionary<Type, DenseArray<Filter>> filters, Type type, int capacity)
        {
            if (filters.TryGetValue(type, out var array))
            {
                array.ExtendTo(capacity);
                return;
            }

            filters.Add(type, new DenseArray<Filter>(capacity));
        }

        private static void Add(Dictionary<Type, DenseArray<Filter>> filters, Filter filter, Type type, int capacity)
        {
            if (filters.TryGetValue(type, out var array))
            {
                array.Add(filter);
                return;
            }

            filters.Add(type, new DenseArray<Filter>(capacity).Add(filter));
        }

        private static void Change(Dictionary<Type, DenseArray<Filter>> filters, Type type, Entity entity, int difference)
        {
            if (filters.TryGetValue(type, out var array))
            {
                foreach (var filter in array)
                {
                    filter.ChangeUnchecked(entity, difference);
                }
            }
        }
    }
}