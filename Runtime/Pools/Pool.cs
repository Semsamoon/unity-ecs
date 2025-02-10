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

        public Entity this[int index] => _denseArray[index];

        public Pool(World world, Type type) : this(world, type, OptionsPool.Default())
        {
        }

        public Pool(World world, Type type, OptionsPool options)
        {
            options = options.Validate();
            _world = world;
            _type = type;
            _sparseArray = new SparseArray<int>(options.EntitiesCapacity);
            _denseArray = new DenseArray<Entity>(options.Capacity);
        }

        public void Add(Entity entity)
        {
            if (!_world.EntitiesInternal.Contains(entity) || Contains(entity))
            {
                return;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            _world.FiltersInternal.RecordUnchecked(entity, _type);
            _world.EntitiesInternal.RecordUnchecked(entity, _type);
        }

        public bool Contains(Entity entity)
        {
            return entity != Entity.Null && _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public void Remove(Entity entity)
        {
            if (!Contains(entity))
            {
                return;
            }

            RemoveUnchecked(entity);
            _world.FiltersInternal.EraseUnchecked(entity, _type);
            _world.EntitiesInternal.EraseUnchecked(entity, _type);
        }

        public void RemoveUnchecked(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index] = new Entity();
            _denseArray.RemoveAt(index);
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

        public (Entity Entity, T Value) this[int index] => _denseArray[index];

        public Pool(World world) : this(world, OptionsPool.Default())
        {
        }

        public Pool(World world, OptionsPool options)
        {
            options = options.Validate();
            _world = world;
            _sparseArray = new SparseArray<int>(options.EntitiesCapacity);
            _denseArray = new DenseArray<(Entity, T)>(options.Capacity);
        }

        public void Set(Entity entity, T value)
        {
            if (!_world.EntitiesInternal.Contains(entity))
            {
                return;
            }

            if (Contains(entity))
            {
                _denseArray[_sparseArray[entity.Id]].Value = value;
                return;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, value));
            _world.FiltersInternal.RecordUnchecked(entity, typeof(T));
            _world.EntitiesInternal.RecordUnchecked(entity, typeof(T));
        }

        public ref T Get(int index)
        {
            return ref _denseArray[index].Value;
        }

        public ref T Get(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity == Entity.Null || _denseArray[index].Entity == entity)
            {
                return ref _denseArray[index].Value;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, default));
            _world.FiltersInternal.RecordUnchecked(entity, typeof(T));
            _world.EntitiesInternal.RecordUnchecked(entity, typeof(T));

            return ref _denseArray[^1].Value;
        }

        public bool Contains(Entity entity)
        {
            return entity != Entity.Null && _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        public void Remove(Entity entity)
        {
            if (!Contains(entity))
            {
                return;
            }

            RemoveUnchecked(entity);
            _world.FiltersInternal.EraseUnchecked(entity, typeof(T));
            _world.EntitiesInternal.EraseUnchecked(entity, typeof(T));
        }

        public void RemoveUnchecked(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            _sparseArray[_denseArray[^1].Entity.Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index].Entity = new Entity();
            _denseArray.RemoveAt(index);
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