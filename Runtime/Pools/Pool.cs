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
            _denseArray.Add(new Entity());
        }

        public Pool(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
            _denseArray.Add(new Entity());
        }

        public void Add(Entity entity)
        {
            if (_sparseArray[entity.Id] > 0)
            {
                return;
            }

            _sparseArray[entity.Id] = _denseArray.Length;
            _denseArray.Add(entity);
        }

        public bool Contains(Entity entity)
        {
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (index <= 0)
            {
                return;
            }

            var backSwappedEntity = _denseArray[^1];
            _denseArray.RemoveAt(index);
            _sparseArray[backSwappedEntity.Id] = index;
            _sparseArray[entity.Id] = 0;
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
            _denseArray.Add((new Entity(), default));
        }

        public Pool(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<(Entity, T)>(denseCapacity);
            _denseArray.Add((new Entity(), default));
        }

        public void AddOrSet(Entity entity, T value)
        {
            if (_sparseArray[entity.Id] > 0)
            {
                _denseArray[_sparseArray[entity.Id]].Value = value;
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
            return _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (index <= 0)
            {
                return;
            }

            var backSwappedEntity = _denseArray[^1].Entity;
            _denseArray.RemoveAt(index);
            _sparseArray[backSwappedEntity.Id] = index;
            _sparseArray[entity.Id] = 0;
        }

        public ReadOnlySpan<(Entity Entity, T Value)> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }
    }
}