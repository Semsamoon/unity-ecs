using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Pools is a manager for pools.
    /// </summary>
    public sealed class Pools : IPools
    {
        private readonly World _world;
        private readonly Dictionary<Type, IPoolInternal> _pools;
        private readonly OptionsPool _defaultOptionsPool;

        public int Length => _pools.Count;

        public Pools(World world) : this(world, OptionsPools.Default(), OptionsPool.Default())
        {
        }

        public Pools(World world, OptionsPools options, OptionsPool optionsPool)
        {
            options = options.Validate();
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(options.Capacity);
            _defaultOptionsPool = optionsPool;
        }

        public IPools Add<T>()
        {
            Add<T>(_defaultOptionsPool);
            return this;
        }

        public IPools Add<T>(OptionsPool options)
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool(_world, typeof(T), options)
                : new Pool<T>(_world, options));
            return this;
        }

        public IPool<T> Get<T>()
        {
            return Get<T>(_defaultOptionsPool);
        }

        public IPool<T> Get<T>(OptionsPool options)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world, options);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPool<T> GetUnchecked<T>()
        {
            return _pools[typeof(T)] as Pool<T>;
        }

        public IPool GetTag<T>() where T : ITag
        {
            return GetTag<T>(_defaultOptionsPool);
        }

        public IPool GetTag<T>(OptionsPool options) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T), options);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPool GetTagUnchecked<T>() where T : ITag
        {
            return _pools[typeof(T)] as IPool;
        }

        public IPoolInternal GetPool<T>()
        {
            return GetPool<T>(_defaultOptionsPool);
        }

        public IPoolInternal GetPool<T>(OptionsPool options)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(_world, typeof(T), options);
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(_world, options);
            _pools.Add(typeof(T), poolT);
            return poolT;
        }

        public IPoolInternal GetPoolUnchecked(Type type)
        {
            return _pools[type];
        }

        public bool Contains<T>()
        {
            return _pools.ContainsKey(typeof(T));
        }

        public IEnumerator<IPoolInternal> GetEnumerator()
        {
            return _pools.Values.GetEnumerator();
        }
    }
}