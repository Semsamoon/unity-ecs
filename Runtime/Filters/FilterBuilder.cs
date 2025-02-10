using System;

namespace ECS
{
    public readonly struct FilterBuilder : IFilterBuilder
    {
        private readonly World _world;
        private readonly DenseArray<(Type, IPoolInternal)> _included;
        private readonly DenseArray<(Type, IPoolInternal)> _excluded;
        private readonly OptionsFilter _defaultOptionsFilter;

        public FilterBuilder(World world, OptionsFilter optionsFilter) : this(world, optionsFilter, OptionsFilterBuilder.Default())
        {
        }

        public FilterBuilder(World world, OptionsFilter optionsFilter, OptionsFilterBuilder optionsBuilder)
        {
            _world = world;
            _included = new DenseArray<(Type, IPoolInternal)>(optionsBuilder.IncludedCapacity);
            _excluded = new DenseArray<(Type, IPoolInternal)>(optionsBuilder.ExcludedCapacity);
            _defaultOptionsFilter = optionsFilter;
        }

        public IFilterBuilder Include<T>()
        {
            _included.Add((typeof(T), _world.PoolsInternal.GetPool<T>()));
            return this;
        }

        public IFilterBuilder Include<T1, T2>()
        {
            _included.Add((typeof(T1), _world.PoolsInternal.GetPool<T1>()));
            _included.Add((typeof(T2), _world.PoolsInternal.GetPool<T2>()));
            return this;
        }

        public IFilterBuilder Include<T1, T2, T3>()
        {
            _included.Add((typeof(T1), _world.PoolsInternal.GetPool<T1>()));
            _included.Add((typeof(T2), _world.PoolsInternal.GetPool<T2>()));
            _included.Add((typeof(T3), _world.PoolsInternal.GetPool<T3>()));
            return this;
        }

        public IFilterBuilder Exclude<T>()
        {
            _excluded.Add((typeof(T), _world.PoolsInternal.GetPool<T>()));
            return this;
        }

        public IFilterBuilder Exclude<T1, T2>()
        {
            _excluded.Add((typeof(T1), _world.PoolsInternal.GetPool<T1>()));
            _excluded.Add((typeof(T2), _world.PoolsInternal.GetPool<T2>()));
            return this;
        }

        public IFilterBuilder Exclude<T1, T2, T3>()
        {
            _excluded.Add((typeof(T1), _world.PoolsInternal.GetPool<T1>()));
            _excluded.Add((typeof(T2), _world.PoolsInternal.GetPool<T2>()));
            _excluded.Add((typeof(T3), _world.PoolsInternal.GetPool<T3>()));
            return this;
        }

        public IFilter Build()
        {
            return Build(_defaultOptionsFilter);
        }

        public IFilter Build(OptionsFilter options)
        {
            var filter = new Filter(_included.Length + _excluded.Length, options);

            foreach (var (type, pool) in _included)
            {
                _world.FiltersInternal.Include(filter, type);

                foreach (var (entity, _) in _world.EntitiesInternal)
                {
                    if (pool.Contains(entity))
                    {
                        filter.ChangeUnchecked(entity, 1);
                    }
                }
            }

            foreach (var (type, pool) in _excluded)
            {
                _world.FiltersInternal.Exclude(filter, type);

                foreach (var (entity, _) in _world.EntitiesInternal)
                {
                    if (!pool.Contains(entity))
                    {
                        filter.ChangeUnchecked(entity, 1);
                    }
                }
            }

            return filter;
        }
    }
}