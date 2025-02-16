using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Pool is a Sparse Set for entities.
    /// Use <see cref="Pool{T}"/> to store data.
    /// </summary>
    public sealed class Pool : IPool, IPoolInternal
    {
        private readonly World _world;
        private readonly Type _type;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        Entity IPool.this[int index]
        {
            get
            {
                Verifier.ArgumentError(nameof(index), index < Length, $"must be less than Length {Length}.");
                return _denseArray[index];
            }
        }

        public Entity this[int index] => _denseArray[index];

        public Pool(World world, Type type) : this(world, type, OptionsPool.Default, OptionsEntities.Default)
        {
        }

        public Pool(World world, Type type, in OptionsPool poolOptions, in OptionsEntities entitiesOptions)
        {
            _world = world;
            _type = type;
            _sparseArray = new SparseArray<int>(entitiesOptions.Capacity);
            _denseArray = new DenseArray<Entity>(poolOptions.Capacity);
        }

        IPool IPool.Add(Entity entity)
        {
            Verifier.EntityExists(entity, _world.Entities);
            Verifier.EntityNotInPool(entity, this, _type);
            return Add(entity);
        }

        public Pool Add(Entity entity)
        {
            AddUnchecked(entity);
            _world.Filters.RecordUnchecked(entity, _type);
            _world.Entities.RecordUnchecked(entity, _type);
            return this;
        }

        public Pool AddUnchecked(Entity entity)
        {
            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            return this;
        }

        bool IPool.Contains(Entity entity)
        {
            Verifier.EntityNotNull(entity);
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public bool Contains(Entity entity)
        {
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        IPool IPool.Remove(Entity entity)
        {
            Verifier.EntityExists(entity, _world.Entities);
            Verifier.EntityInPool(entity, this, _type);
            return Remove(entity);
        }

        public Pool Remove(Entity entity)
        {
            RemoveUnchecked(entity);
            _world.Filters.EraseUnchecked(entity, _type);
            _world.Entities.EraseUnchecked(entity, _type);
            return this;
        }

        IPoolInternal IPoolInternal.RemoveUnchecked(Entity entity)
        {
            return RemoveUnchecked(entity);
        }

        public Pool RemoveUnchecked(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index] = new Entity();
            _denseArray.RemoveAt(index);
            return this;
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
    public sealed class Pool<T> : IPool<T>, IPoolInternal
    {
        private readonly World _world;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<(Entity Entity, T Value)> _denseArray;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        Entity IPool<T>.this[int index]
        {
            get
            {
                Verifier.ArgumentError(nameof(index), index < Length, $"must be less than Length {Length}.");
                return _denseArray[index].Entity;
            }
        }

        public Entity this[int index] => _denseArray[index].Entity;

        public Pool(World world) : this(world, OptionsPool.Default, OptionsEntities.Default)
        {
        }

        public Pool(World world, in OptionsPool poolOptions, in OptionsEntities entitiesOptions)
        {
            _world = world;
            _sparseArray = new SparseArray<int>(entitiesOptions.Capacity);
            _denseArray = new DenseArray<(Entity, T)>(poolOptions.Capacity);
        }

        IPool<T> IPool<T>.Set(Entity entity, T value)
        {
            Verifier.EntityExists(entity, _world.Entities);
            Get(entity) = value;
            return this;
        }

        public Pool<T> Set(Entity entity, T value)
        {
            Get(entity) = value;
            return this;
        }

        public Pool<T> SetUnchecked(Entity entity, T value)
        {
            GetUnchecked(entity) = value;
            return this;
        }

        ref T IPool<T>.Get(Entity entity)
        {
            Verifier.EntityExists(entity, _world.Entities);
            return ref Get(entity);
        }

        public ref T Get(Entity entity)
        {
            if (Contains(entity))
            {
                return ref GetUnchecked(entity);
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, default));
            _world.Filters.RecordUnchecked(entity, typeof(T));
            _world.Entities.RecordUnchecked(entity, typeof(T));

            return ref _denseArray[^1].Value;
        }

        public ref T GetUnchecked(Entity entity)
        {
            return ref _denseArray[_sparseArray[entity.Id]].Value;
        }

        ref T IPool<T>.Get(int index)
        {
            Verifier.ArgumentError(nameof(index), index < Length, $"must be less than Length {Length}.");
            return ref _denseArray[index].Value;
        }

        public ref T Get(int index)
        {
            return ref _denseArray[index].Value;
        }

        bool IPool<T>.Contains(Entity entity)
        {
            Verifier.EntityNotNull(entity);
            return _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        public bool Contains(Entity entity)
        {
            return _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        IPool<T> IPool<T>.Remove(Entity entity)
        {
            Verifier.EntityExists(entity, _world.Entities);
            Verifier.EntityInPool(entity, this, typeof(T));
            return Remove(entity);
        }

        public Pool<T> Remove(Entity entity)
        {
            RemoveUnchecked(entity);
            _world.Filters.EraseUnchecked(entity, typeof(T));
            _world.Entities.EraseUnchecked(entity, typeof(T));
            return this;
        }

        IPoolInternal IPoolInternal.RemoveUnchecked(Entity entity)
        {
            return RemoveUnchecked(entity);
        }

        public Pool<T> RemoveUnchecked(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            _sparseArray[_denseArray[^1].Entity.Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index].Entity = new Entity();
            _denseArray.RemoveAt(index);
            return this;
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