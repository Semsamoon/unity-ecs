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

        public Filters(World world)
        {
            _world = world;
            _included = new Dictionary<Type, DenseArray<Filter>>(OptionsFilters.DefaultCapacity);
            _excluded = new Dictionary<Type, DenseArray<Filter>>(OptionsFilters.DefaultCapacity);
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

        public FilterBuilder Create()
        {
            return new FilterBuilder(this, _world.PoolsInternal, _world.EntitiesInternal, _defaultOptionsFilter);
        }

        public FilterBuilder Create(int included, int excluded)
        {
            return new FilterBuilder(this, _world.PoolsInternal, _world.EntitiesInternal, _defaultOptionsFilter, included, excluded);
        }

        public void Include(Filter filter, Type type)
        {
            if (_included.TryGetValue(type, out var included))
            {
                included.Add(filter);
                return;
            }

            var filters = new DenseArray<Filter>(_defaultFiltersCapacity);
            filters.Add(filter);
            _included.Add(type, filters);
        }

        public void Include(Filter filter, Type type, int capacity)
        {
            if (_included.TryGetValue(type, out var included))
            {
                included.Add(filter);
                return;
            }

            var filters = new DenseArray<Filter>(capacity > 0 ? capacity : _defaultFiltersCapacity);
            filters.Add(filter);
            _included.Add(type, filters);
        }

        public void Exclude(Filter filter, Type type)
        {
            if (_excluded.TryGetValue(type, out var excluded))
            {
                excluded.Add(filter);
                return;
            }

            var filters = new DenseArray<Filter>(_defaultFiltersCapacity);
            filters.Add(filter);
            _excluded.Add(type, filters);
        }

        public void Exclude(Filter filter, Type type, int capacity)
        {
            if (_excluded.TryGetValue(type, out var excluded))
            {
                excluded.Add(filter);
                return;
            }

            var filters = new DenseArray<Filter>(capacity > 0 ? capacity : _defaultFiltersCapacity);
            filters.Add(filter);
            _excluded.Add(type, filters);
        }

        public void Record(Entity entity, Type type)
        {
            if (_included.TryGetValue(type, out var included))
            {
                foreach (var filter in included)
                {
                    filter.Change(entity, 1);
                }
            }

            if (_excluded.TryGetValue(type, out var excluded))
            {
                foreach (var filter in excluded)
                {
                    filter.Change(entity, -1);
                }
            }
        }

        public void Erase(Entity entity, Type type)
        {
            if (_included.TryGetValue(type, out var included))
            {
                foreach (var filter in included)
                {
                    filter.Change(entity, -1);
                }
            }

            if (_excluded.TryGetValue(type, out var excluded))
            {
                foreach (var filter in excluded)
                {
                    filter.Change(entity, 1);
                }
            }
        }
    }
}