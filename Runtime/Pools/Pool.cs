using System;

namespace ECS
{
    /// <summary>
    /// Pool is a Sparse Set for entities.
    /// Use <see cref="Pool{T}"/> to store data.
    /// </summary>
    public sealed class Pool
    {
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Pool()
        {
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<Entity>();
        }

        public Pool(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
        }

        public void Add(Entity entity)
        {
            if (entity.IsNull() || _denseArray[_sparseArray[entity.Id]] == entity)
            {
                return;
            }

            _sparseArray[entity.Id] = _denseArray.Length;
            _denseArray.Add(entity);
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
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }
    }

    /// <summary>
    /// Pool is a Sparse Set for entities with data.
    /// Use <see cref="Pool"/> to store entities only.
    /// </summary>
    public sealed class Pool<T>
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<(Entity Entity, T Value)> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index].Entity;

        public Pool()
        {
            _sparseArray = new SparseArray<int>(DefaultSparseCapacity);
            _denseArray = new DenseArray<(Entity, T)>(DefaultDenseCapacity);
        }

        public Pool(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<(Entity, T)>(denseCapacity);
        }

        public void AddOrSet(Entity entity, T value)
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

            _sparseArray[entity.Id] = _denseArray.Length;
            _denseArray.Add((entity, value));
        }

        public ref T Get(int index)
        {
            return ref _denseArray[index].Value;
        }

        public ref T Get(Entity entity)
        {
            return ref _denseArray[_sparseArray[entity.Id]].Value;
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
        }

        public ReadOnlySpan<(Entity Entity, T Value)> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }
    }
}