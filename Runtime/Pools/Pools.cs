using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Pools is a manager for pools.
    /// </summary>
    public sealed class Pools : IPools
    {
        private const int DefaultCapacity = 64;

        private readonly World _world;
        private readonly Dictionary<Type, IPoolInternal> _pools;

        public int Length => _pools.Count;

        public Pools(World world)
        {
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(DefaultCapacity);
        }

        public Pools(World world, int capacity)
        {
            capacity = Math.Max(capacity, 2);
            _world = world;
            _pools = new Dictionary<Type, IPoolInternal>(capacity);
        }

        public IPools Add<T>()
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool(_world, typeof(T))
                : new Pool<T>(_world));
            return this;
        }

        public IPools Add<T>(int sparseCapacity, int denseCapacity)
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool(_world, typeof(T), sparseCapacity, denseCapacity)
                : new Pool<T>(_world, sparseCapacity, denseCapacity));
            return this;
        }

        public IPool<T> Get<T>()
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPool<T> Get<T>(int sparseCapacity, int denseCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(_world, sparseCapacity, denseCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPool GetTag<T>() where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T));
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPool GetTag<T>(int sparseCapacity, int denseCapacity) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(_world, typeof(T), sparseCapacity, denseCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public IPoolInternal GetPool<T>()
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(_world, typeof(T));
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(_world);
            _pools.Add(typeof(T), poolT);
            return poolT;
        }

        public IPoolInternal GetPool<T>(int sparseCapacity, int denseCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(_world, typeof(T), sparseCapacity, denseCapacity);
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(_world, sparseCapacity, denseCapacity);
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