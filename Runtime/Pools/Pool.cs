using System;

namespace ECS
{
    public sealed class Pool
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Pool(int sparseCapacity = DefaultSparseCapacity, int denseCapacity = DefaultDenseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
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
            _sparseArray[entity.Id] = 0;
            _sparseArray[backSwappedEntity.Id] = index;
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }
    }

    public sealed class Pool<T>
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;
        private readonly DenseArray<T> _itemsArray;

        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];
        public ref T this[Entity entity] => ref _itemsArray[_sparseArray[entity.Id]];

        public Pool(int sparseCapacity = DefaultSparseCapacity, int denseCapacity = DefaultDenseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
            _itemsArray = new DenseArray<T>(denseCapacity);
        }

        public void AddOrSet(Entity entity, T item)
        {
            if (_sparseArray[entity.Id] > 0)
            {
                _itemsArray[_sparseArray[entity.Id]] = item;
                return;
            }

            _sparseArray[entity.Id] = _denseArray.Length;
            _denseArray.Add(entity);
            _itemsArray.Add(item);
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
            _itemsArray.RemoveAt(index);
            _sparseArray[entity.Id] = 0;
            _sparseArray[backSwappedEntity.Id] = index;
        }

        public ReadOnlySpan<Entity> DenseAsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public ReadOnlySpan<T> ItemsAsReadOnlySpan()
        {
            return _itemsArray.AsReadOnlySpan();
        }
    }
}