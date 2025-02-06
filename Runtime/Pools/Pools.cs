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

        private readonly Dictionary<Type, IPool> _pools;

        public int Length => _pools.Count;

        public Pools()
        {
            _pools = new Dictionary<Type, IPool>(DefaultCapacity);
        }

        public Pools(int capacity)
        {
            capacity = Math.Max(capacity, 2);
            _pools = new Dictionary<Type, IPool>(capacity);
        }

        public IPools Add<T>()
        {
            _pools.TryAdd(typeof(T), typeof(ITag).IsAssignableFrom(typeof(T))
                ? new Pool()
                : new Pool<T>());
            return this;
        }

        public IPools Add<T>(int sparseCapacity, int denseCapacity)
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

        public IPool GetPool<T>()
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool();
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>();
            _pools.Add(typeof(T), poolT);
            return poolT;
        }

        public IPool GetPool<T>(int sparseCapacity, int denseCapacity)
        {
            if (_pools.TryGetValue(typeof(T), out var existing))
            {
                return existing;
            }

            if (typeof(T).IsAssignableFrom(typeof(ITag)))
            {
                var pool = new Pool(sparseCapacity, denseCapacity);
                _pools.Add(typeof(T), pool);
                return pool;
            }

            var poolT = new Pool<T>(sparseCapacity, denseCapacity);
            _pools.Add(typeof(T), poolT);
            return poolT;
        }

        public bool Contains<T>()
        {
            return _pools.ContainsKey(typeof(T));
        }

        public IEnumerator<IPool> GetEnumerator()
        {
            return _pools.Values.GetEnumerator();
        }
    }
}