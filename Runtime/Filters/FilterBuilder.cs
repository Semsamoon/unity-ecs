using System;

namespace ECS
{
    public readonly struct FilterBuilder
    {
        private const int DefaultCapacity = 4;

        private readonly Filters _filters;
        private readonly Pools _pools;
        private readonly Entities _entities;
        private readonly DenseArray<(Type, IPool)> _included;
        private readonly DenseArray<(Type, IPool)> _excluded;

        public FilterBuilder(Filters filters, Pools pools, Entities entities)
        {
            _filters = filters;
            _pools = pools;
            _entities = entities;
            _included = new DenseArray<(Type, IPool)>(DefaultCapacity);
            _excluded = new DenseArray<(Type, IPool)>(DefaultCapacity);
        }

        public FilterBuilder(Filters filters, Pools pools, Entities entities, int included, int excluded)
        {
            _filters = filters;
            _pools = pools;
            _entities = entities;
            _included = new DenseArray<(Type, IPool)>(included);
            _excluded = new DenseArray<(Type, IPool)>(excluded);
        }

        public FilterBuilder Include<T>()
        {
            _included.Add((typeof(T), _pools.GetPool<T>()));
            return this;
        }

        public FilterBuilder Include<T1, T2>()
        {
            _included.Add((typeof(T1), _pools.GetPool<T1>()));
            _included.Add((typeof(T2), _pools.GetPool<T2>()));
            return this;
        }

        public FilterBuilder Include<T1, T2, T3>()
        {
            _included.Add((typeof(T1), _pools.GetPool<T1>()));
            _included.Add((typeof(T2), _pools.GetPool<T2>()));
            _included.Add((typeof(T3), _pools.GetPool<T3>()));
            return this;
        }

        public FilterBuilder Exclude<T>()
        {
            _excluded.Add((typeof(T), _pools.GetPool<T>()));
            return this;
        }

        public FilterBuilder Exclude<T1, T2>()
        {
            _excluded.Add((typeof(T1), _pools.GetPool<T1>()));
            _excluded.Add((typeof(T2), _pools.GetPool<T2>()));
            return this;
        }

        public FilterBuilder Exclude<T1, T2, T3>()
        {
            _excluded.Add((typeof(T1), _pools.GetPool<T1>()));
            _excluded.Add((typeof(T2), _pools.GetPool<T2>()));
            _excluded.Add((typeof(T3), _pools.GetPool<T3>()));
            return this;
        }

        public IFilter Build()
        {
            var filter = new Filter(_included.Length + _excluded.Length);

            foreach (var (type, pool) in _included)
            {
                _filters.Include(filter, type);

                foreach (var entity in _entities)
                {
                    if (pool.Contains(entity))
                    {
                        filter.Change(entity, 1);
                    }
                }
            }

            foreach (var (type, pool) in _excluded)
            {
                _filters.Exclude(filter, type);

                foreach (var entity in _entities)
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