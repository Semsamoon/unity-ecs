using System;

namespace ECS
{
    /// <summary>
    /// A pool to store entities. Consists of <see cref="SparseArray{T}"/> with
    /// indexes and <see cref="DenseArray{T}"/> with entities. Index for sparse array
    /// is an <see cref="Entity"/> <see cref="Entity.Id"/>. Value of sparse array
    /// is an index for dense array. Value of dense array is the entity itself.<br/>
    /// <br/>
    /// <i>Pools are containers for some data placed consistently in memory.
    /// This type of Pools can only store entities. Use <see cref="Pool{T}"/>
    /// to store components.</i>
    /// </summary>
    public sealed class Pool
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        /// <summary>
        /// Current length of internal <see cref="DenseArray{T}"/>.
        /// </summary>
        public int Length => _denseArray.Length;

        /// <summary>
        /// Access to entities in internal <see cref="DenseArray{T}"/>.
        /// </summary>
        public Entity this[int index] => _denseArray[index];

        /// <param name="sparseCapacity">Initial capacity of internal <see cref="SparseArray{T}"/></param>
        /// <param name="denseCapacity">Initial capacity of internal <see cref="DenseArray{T}"/></param>
        public Pool(int sparseCapacity = DefaultSparseCapacity, int denseCapacity = DefaultDenseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
        }

        /// <summary>
        /// Adds the <paramref name="entity"/> to the end of internal <see cref="DenseArray{T}"/>.
        /// Does nothing if it has already been added.
        /// </summary>
        public void Add(Entity entity)
        {
            if (_sparseArray[entity.Id] > 0)
            {
                return;
            }

            _sparseArray[entity.Id] = _denseArray.Length;
            _denseArray.Add(entity);
        }

        /// <returns>True if the <paramref name="entity"/> has been added
        /// to internal <see cref="DenseArray{T}"/>, false otherwise</returns>
        public bool Contains(Entity entity)
        {
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        /// <summary>
        /// Removes the <paramref name="entity"/> from internal <see cref="DenseArray{T}"/>.
        /// Does nothing if it has not been added yet.
        /// </summary>
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

    /// <summary>
    /// A pool to store entities with components. Consists of <see cref="SparseArray{T}"/>
    /// with indexes and two <see cref="DenseArray{T}"/> with entities (dense array) and
    /// components (items array). Index for sparse array is an <see cref="Entity"/>
    /// <see cref="Entity.Id"/>. Value of sparse array is an index for both dense and items
    /// arrays. Value of dense array is the entity itself. Value of items array is a
    /// component associated with the entity.<br/>
    /// <br/>
    /// <i>Pools are containers for some data placed consistently in memory.
    /// This type of Pools can store entities with components. Use <see cref="Pool"/>
    /// to store entities only.</i>
    /// </summary>
    /// <typeparam name="T">Type of components to store</typeparam>
    public sealed class Pool<T>
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;
        private readonly DenseArray<T> _itemsArray;

        /// <summary>
        /// Current length of internal <see cref="DenseArray{T}"/> (both dense and items arrays).
        /// </summary>
        public int Length => _denseArray.Length;

        /// <summary>
        /// Access to entities in internal <see cref="DenseArray{T}"/>.
        /// </summary>
        public Entity this[int index] => _denseArray[index];

        /// <summary>
        /// Access to components in internal <see cref="DenseArray{T}"/> by entity.
        /// </summary>
        public ref T this[Entity entity] => ref _itemsArray[_sparseArray[entity.Id]];

        /// <param name="sparseCapacity">Initial capacity of internal <see cref="SparseArray{T}"/></param>
        /// <param name="denseCapacity">Initial capacity of internal <see cref="DenseArray{T}"/></param>
        public Pool(int sparseCapacity = DefaultSparseCapacity, int denseCapacity = DefaultDenseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
            _itemsArray = new DenseArray<T>(denseCapacity);
        }

        /// <summary>
        /// Adds the <paramref name="item"/> with the <paramref name="entity"/> to
        /// the end of internal <see cref="DenseArray{T}"/>. Overwrites existing
        /// item if it has already been added.
        /// </summary>
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

        /// <returns>True if the <paramref name="entity"/> with associated component has
        /// been added to internal <see cref="DenseArray{T}"/>, false otherwise</returns>
        public bool Contains(Entity entity)
        {
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        /// <summary>
        /// Removes the <paramref name="entity"/> with associated component from internal
        /// <see cref="DenseArray{T}"/>. Does nothing if it has not been added yet.
        /// </summary>
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