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
        private readonly int _defaultFiltersCapacity;
        private readonly OptionsFilter _defaultOptionsFilter;

        public (int included, int excluded) Length => (_included.Count, _excluded.Count);

        public Filters(World world) : this(world, OptionsFilters.Default(), OptionsFilter.Default())
        {
        }

        public Filters(World world, OptionsFilters options, OptionsFilter optionsFilter)
        {
            options = options.Validate();
            _world = world;
            _included = new Dictionary<Type, DenseArray<Filter>>(options.Capacity);
            _excluded = new Dictionary<Type, DenseArray<Filter>>(options.Capacity);
            _defaultFiltersCapacity = options.FiltersCapacity;
            _defaultOptionsFilter = optionsFilter;
        }

        public IFilterBuilderEmpty Create()
        {
            return new FilterBuilder(_world, _defaultOptionsFilter);
        }

        public IFilterBuilderEmpty Create(OptionsFilter options)
        {
            return new FilterBuilder(_world, options);
        }

        public IFilters IncludeCapacity<T>(int capacity)
        {
            capacity = capacity > 0 ? capacity : _defaultFiltersCapacity;
            EnsureCapacity(_included, typeof(T), capacity);
            return this;
        }

        public IFilters ExcludeCapacity<T>(int capacity)
        {
            capacity = capacity > 0 ? capacity : _defaultFiltersCapacity;
            EnsureCapacity(_excluded, typeof(T), capacity);
            return this;
        }

        public Filters Include(Filter filter, Type type)
        {
            return Include(filter, type, _defaultFiltersCapacity);
        }

        public Filters Include(Filter filter, Type type, int capacity)
        {
            capacity = capacity > 0 ? capacity : _defaultFiltersCapacity;
            Add(_included, filter, type, capacity);
            return this;
        }

        public Filters Exclude(Filter filter, Type type)
        {
            return Exclude(filter, type, _defaultFiltersCapacity);
        }

        public Filters Exclude(Filter filter, Type type, int capacity)
        {
            capacity = capacity > 0 ? capacity : _defaultFiltersCapacity;
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