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
        private readonly OptionsPool _defaultPoolOptions;
        private readonly OptionsEntities _entitiesOptions;

        public int Length => _pools.Count;

        public Pools(World world) : this(world, OptionsPools.Default, OptionsPool.Default, OptionsEntities.Default)
        {
        }

        public Pools(World world, in OptionsPools poolsOptions, in OptionsPool poolOptions, in OptionsEntities entitiesOptions)
        {
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(poolsOptions.Capacity);
            _defaultPoolOptions = poolOptions;
            _entitiesOptions = entitiesOptions;
        }

        IPools IPools.Add<T>()
        {
            return Add<T>(in _defaultPoolOptions);
        }

        public Pools Add<T>()
        {
            return Add<T>(in _defaultPoolOptions);
        }

        IPools IPools.Add<T>(in OptionsPool options)
        {
            return Add<T>(in options);
        }

        public Pools Add<T>(in OptionsPool options)
        {
            if (_pools.ContainsKey(typeof(T)))
            {
                return this;
            }

            if (typeof(ITag).IsAssignableFrom(typeof(T)))
            {
                _pools.Add(typeof(T), new Pool(_world, typeof(T), in options, in _entitiesOptions));
                return this;
            }

            _pools.Add(typeof(T), new Pool<T>(_world, in options, in _entitiesOptions));
            return this;
        }

        IPool<T> IPools.Get<T>()
        {
            return Get<T>(in _defaultPoolOptions);
        }

        public Pool<T> Get<T>()
        {
            return Get<T>(in _defaultPoolOptions);
        }

        IPool<T> IPools.Get<T>(in OptionsPool options)
        {
            return Get<T>(in options);
        }

        public Pool<T> Get<T>(in OptionsPool options)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world, in options, _entitiesOptions);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool<T> GetUnchecked<T>()
        {
            return (Pool<T>)_pools[typeof(T)];
        }

        IPool IPools.GetTag<T>()
        {
            return GetTag<T>(in _defaultPoolOptions);
        }

        public Pool GetTag<T>() where T : ITag
        {
            return GetTag<T>(in _defaultPoolOptions);
        }

        IPool IPools.GetTag<T>(in OptionsPool options)
        {
            return GetTag<T>(in options);
        }

        public Pool GetTag<T>(in OptionsPool options) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T), in options, in _entitiesOptions);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool GetTagUnchecked<T>() where T : ITag
        {
            return (Pool)_pools[typeof(T)];
        }

        public IPoolInternal GetPool<T>()
        {
            return GetPool<T>(in _defaultPoolOptions);
        }

        public IPoolInternal GetPool<T>(in OptionsPool options)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(_world, typeof(T), in options, in _entitiesOptions);
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(_world, in options, in _entitiesOptions);
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