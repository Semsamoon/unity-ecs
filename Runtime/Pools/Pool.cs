using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Pool is a Sparse Set for entities.
    /// Use <see cref="Pool{T}"/> to store data.
    /// </summary>
    public sealed class Pool : IPool
    {
        private readonly World _world;
        private readonly Type _type;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Pool(World world, Type type)
        {
            _world = world;
            _type = type;
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<Entity>();
        }

        public Pool(World world, Type type, int sparseCapacity, int denseCapacity)
        {
            _world = world;
            _type = type;
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
        }

        public void Add(Entity entity)
        {
            if (entity.IsNull() || _denseArray[_sparseArray[entity.Id]] == entity)
            {
                return;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            _world.FiltersInternal.Record(entity, _type);
        }

        public bool Contains(Entity entity)
        {
            return !entity.IsNull() && _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity.IsNull() || _denseArray[index] != entity)
            {
                return;
            }

            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index] = new Entity();
            _denseArray.RemoveAt(index);
            _world.FiltersInternal.Erase(entity, _type);
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _denseArray.GetEnumerator();
        }
    }

    /// <summary>
    /// Pool is a Sparse Set for entities with data.
    /// Use <see cref="Pool"/> to store entities only.
    /// </summary>
    public sealed class Pool<T> : IPool
    {
        private readonly World _world;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<(Entity Entity, T Value)> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public (Entity Entity, T Value) this[int index] => _denseArray[index];

        public Pool(World world)
        {
            _world = world;
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<(Entity, T)>();
        }

        public Pool(World world, int sparseCapacity, int denseCapacity)
        {
            _world = world;
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<(Entity, T)>(denseCapacity);
        }

        public void Set(Entity entity, T value)
        {
            if (entity.IsNull())
            {
                return;
            }

            ref var data = ref _denseArray[_sparseArray[entity.Id]];

            if (data.Entity == entity)
            {
                data.Value = value;
                return;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, value));
            _world.FiltersInternal.Record(entity, typeof(T));
        }

        public ref T Get(int index)
        {
            return ref _denseArray[index].Value;
        }

        public ref T Get(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity.IsNull() || _denseArray[index].Entity == entity)
            {
                return ref _denseArray[index].Value;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, default));
            _world.FiltersInternal.Record(entity, typeof(T));

            return ref _denseArray[^1].Value;
        }

        public bool Contains(Entity entity)
        {
            return !entity.IsNull() && _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity.IsNull() || _denseArray[index].Entity != entity)
            {
                return;
            }

            _sparseArray[_denseArray[^1].Entity.Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index].Entity = new Entity();
            _denseArray.RemoveAt(index);
            _world.FiltersInternal.Erase(entity, typeof(T));
        }

        public ReadOnlySpan<(Entity Entity, T Value)> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public IEnumerator<(Entity Entity, T Value)> GetEnumerator()
        {
            return _denseArray.GetEnumerator();
        }
    }
}