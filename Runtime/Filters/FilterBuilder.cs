using System;

namespace ECS
{
    public readonly struct FilterBuilder : IFilterBuilder
    {
        private const int DefaultCapacity = 4;

        private readonly World _world;
        private readonly DenseArray<(Type, IPoolInternal)> _included;
        private readonly DenseArray<(Type, IPoolInternal)> _excluded;
        private readonly OptionsFilter _options;

        public FilterBuilder(World world, OptionsFilter options) : this(world, options, DefaultCapacity, DefaultCapacity)
        {
        }

        public FilterBuilder(World world, OptionsFilter options, int included, int excluded)
        {
            _world = world;
            _included = new DenseArray<(Type, IPoolInternal)>(included);
            _excluded = new DenseArray<(Type, IPoolInternal)>(excluded);
            _options = options;
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
            return Build(_options);
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
                        filter.Change(entity, 1);
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
                        filter.Change(entity, 1);
                    }
                }
            }

            return filter;
        }
    }
}