using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Pools is a manager for pools.
    /// </summary>
    public sealed class Pools
    {
        private const int DefaultCapacity = 64;

        private readonly Dictionary<Type, object> _pools;

        public int Length => _pools.Count;

        public Pools()
        {
            _pools = new Dictionary<Type, object>(DefaultCapacity);
        }

        public Pools(int capacity)
        {
            capacity = Math.Max(capacity, 2);
            _pools = new Dictionary<Type, object>(capacity);
        }

        public Pools Add<T>()
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool()
                : new Pool<T>());
            return this;
        }

        public Pools Add<T>(int sparseCapacity, int denseCapacity)
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool(sparseCapacity, denseCapacity)
                : new Pool<T>(sparseCapacity, denseCapacity));
            return this;
        }

        public Pool<T> Get<T>()
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>();
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool<T> Get<T>(int sparseCapacity, int denseCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool<T>)existing;
            }

            var pool = new Pool<T>(sparseCapacity, denseCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool GetTag<T>() where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool();
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public Pool GetTag<T>(int sparseCapacity, int denseCapacity) where T : ITag
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return (Pool)existing;
            }

            var pool = new Pool(sparseCapacity, denseCapacity);
            _pools.Add(typeof(T), pool);
            return pool;
        }

        public bool Contains<T>()
        {
            return _pools.ContainsKey(typeof(T));
        }

        public void Remove<T>()
        {
            _pools.Remove(typeof(T));
        }
    }
}