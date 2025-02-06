using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Filters is a manager for filters.
    /// </summary>
    public sealed class Filters
    {
        private const int DefaultCapacity = 64;
        private const int DefaultFiltersCapacity = 8;

        private readonly Dictionary<Type, DenseArray<Filter>> _included;
        private readonly Dictionary<Type, DenseArray<Filter>> _excluded;

        public (int included, int excluded) Length => (_included.Count, _excluded.Count);

        public Filters()
        {
            _included = new Dictionary<Type, DenseArray<Filter>>(DefaultCapacity);
            _excluded = new Dictionary<Type, DenseArray<Filter>>(DefaultCapacity);
        }

        public Filters(int includedCapacity, int excludedCapacity)
        {
            includedCapacity = Math.Max(includedCapacity, 2);
            excludedCapacity = Math.Max(excludedCapacity, 2);
            _included = new Dictionary<Type, DenseArray<Filter>>(includedCapacity);
            _excluded = new Dictionary<Type, DenseArray<Filter>>(excludedCapacity);
        }

        public void Include(Filter filter, Type type)
        {
            if (_included.TryGetValue(type, out var included))
            {
                included.Add(filter);
                return;
            }

            var filters = new DenseArray<Filter>(DefaultFiltersCapacity);
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

            var filters = new DenseArray<Filter>(capacity);
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

            var filters = new DenseArray<Filter>(DefaultFiltersCapacity);
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

            var filters = new DenseArray<Filter>(capacity);
            filters.Add(filter);
            _excluded.Add(type, filters);
        }

        public void Create(Entity entity, Type type)
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

        public void Remove(Entity entity, Type type)
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