using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Represents a collection of pools in the <see cref="World"/>, managing their lifecycle.
    /// </summary>
    public sealed class Pools : IPools
    {
        private readonly World _world;
        private readonly Dictionary<Type, IPoolInternal> _pools;

        private readonly int _entitiesCapacity;
        private readonly int _poolComponentsCapacity;

        public int Length => _pools.Count;

        public Pools(
            World world,
            int poolsCapacity = Options.DefaultPoolsCapacity,
            int entitiesCapacity = Options.DefaultEntitiesCapacity,
            int poolComponentsCapacity = Options.DefaultPoolComponentsCapacity)
        {
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(poolsCapacity);
            _entitiesCapacity = entitiesCapacity;
            _poolComponentsCapacity = poolComponentsCapacity;
        }

        IPools IPools.Add<T>()
        {
            return Add<T>(_poolComponentsCapacity);
        }

        public Pools Add<T>()
        {
            return Add<T>(_poolComponentsCapacity);
        }

        IPools IPools.Add<T>(int poolComponentsCapacity)
        {
            return Add<T>(poolComponentsCapacity);
        }

        public Pools Add<T>(int poolComponentsCapacity)
        {
            if (_pools.ContainsKey(typeof(T)))
            {
                return this;
            }

            if (typeof(ITag).IsAssignableFrom(typeof(T)))
            {
                _pools.Add(typeof(T), new Pool(_world, typeof(T), _entitiesCapacity, poolComponentsCapacity));
                return this;
            }

            _pools.Add(typeof(T), new Pool<T>(_world, _entitiesCapacity, poolComponentsCapacity));
            return this;
        }

        IPool<T> IPools.Get<T>()
        {
            return Get<T>(_poolComponentsCapacity);
        }

        public Pool<T> Get<T>()
        {
            return Get<T>(_poolComponentsCapacity);
        }

        IPool<T> IPools.Get<T>(int poolComponentsCapacity)
        {
            return Get<T>(poolComponentsCapacity);
        }

        public Pool<T> Get<T>(int poolComponentsCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world, _entitiesCapacity, poolComponentsCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool<T> GetUnchecked<T>()
        {
            return (Pool<T>)_pools[typeof(T)];
        }

        IPool IPools.GetTag<T>()
        {
            return GetTag<T>(_poolComponentsCapacity);
        }

        public Pool GetTag<T>() where T : ITag
        {
            return GetTag<T>(_poolComponentsCapacity);
        }

        IPool IPools.GetTag<T>(int poolComponentsCapacity)
        {
            return GetTag<T>(poolComponentsCapacity);
        }

        public Pool GetTag<T>(int poolComponentsCapacity) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T), _entitiesCapacity, poolComponentsCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool GetTagUnchecked<T>() where T : ITag
        {
            return (Pool)_pools[typeof(T)];
        }

        public IPoolInternal GetPool<T>()
        {
            return GetPool<T>(_poolComponentsCapacity);
        }

        public IPoolInternal GetPool<T>(int poolComponentsCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(_world, typeof(T), _entitiesCapacity, poolComponentsCapacity);
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(_world, _entitiesCapacity, poolComponentsCapacity);
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