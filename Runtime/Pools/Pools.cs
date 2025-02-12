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

        public Pools(World world) : this(world, OptionsPools.Default, OptionsPool.Default)
        {
        }

        public Pools(World world, OptionsPools options, OptionsPool optionsPool)
        {
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(options.Capacity);
            _defaultOptionsPool = optionsPool;
        }

        IPools IPools.Add<T>()
        {
            return Add<T>(_defaultOptionsPool);
        }

        public Pools Add<T>()
        {
            return Add<T>(_defaultOptionsPool);
        }

        IPools IPools.Add<T>(OptionsPool options)
        {
            return Add<T>(options);
        }

        public Pools Add<T>(OptionsPool options)
        {
            if (_pools.ContainsKey(typeof(T)))
            {
                return this;
            }

            if (typeof(ITag).IsAssignableFrom(typeof(T)))
            {
                _pools.Add(typeof(T), new Pool(_world, typeof(T), options));
                return this;
            }

            _pools.Add(typeof(T), new Pool<T>(_world, options));
            return this;
        }

        IPool<T> IPools.Get<T>()
        {
            return Get<T>(_defaultOptionsPool);
        }

        public Pool<T> Get<T>()
        {
            return Get<T>(_defaultOptionsPool);
        }

        IPool<T> IPools.Get<T>(OptionsPool options)
        {
            return Get<T>(options);
        }

        public Pool<T> Get<T>(OptionsPool options)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world, options);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool<T> GetUnchecked<T>()
        {
            return (Pool<T>)_pools[typeof(T)];
        }

        IPool IPools.GetTag<T>()
        {
            return GetTag<T>(_defaultOptionsPool);
        }

        public Pool GetTag<T>() where T : ITag
        {
            return GetTag<T>(_defaultOptionsPool);
        }

        IPool IPools.GetTag<T>(OptionsPool options)
        {
            return GetTag<T>(options);
        }

        public Pool GetTag<T>(OptionsPool options) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T), options);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool GetTagUnchecked<T>() where T : ITag
        {
            return (Pool)_pools[typeof(T)];
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