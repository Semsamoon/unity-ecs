using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Entities is a manager for entities.
    /// </summary>
    public sealed class Entities : IEntities
    {
        private readonly World _world;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<(Entity Entity, ReadOnlyDenseArray<Type> Components)> _denseArray;
        private readonly DenseArray<DenseArray<Type>> _components;
        private readonly int _defaultComponentsCapacity;

        private int _removed;
        private int _id;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public (Entity Entity, ReadOnlyDenseArray<Type> Components) this[int index] => _denseArray[index];

        public Entities(World world) : this(world, OptionsEntities.Default())
        {
        }

        public Entities(World world, OptionsEntities options)
        {
            options = options.Validate();
            _world = world;
            _sparseArray = new SparseArray<int>(options.Capacity);
            _denseArray = new DenseArray<(Entity, ReadOnlyDenseArray<Type>)>(options.Capacity);
            _components = new DenseArray<DenseArray<Type>>(options.Capacity);
            _defaultComponentsCapacity = options.ComponentsCapacity;
        }

        public Entity Create()
        {
            return _removed <= 0 ? CreateUnchecked(_defaultComponentsCapacity) : RecycleUnchecked();
        }

        public Entity Create(int componentsCapacity)
        {
            if (_removed <= 0)
            {
                return CreateUnchecked(componentsCapacity);
            }

            var recycle = RecycleUnchecked();
            _components[_sparseArray[recycle.Id]].ExtendTo(componentsCapacity);
            return recycle;
        }

        public Entity CreateUnchecked(int componentsCapacity)
        {
            var entity = new Entity(_id, 1);
            var denseArray = new DenseArray<Type>(componentsCapacity);

            _sparseArray[_id] = Length;
            _components.Add(denseArray);
            _denseArray.Add((entity, denseArray.AsReadOnly()));
            _id++;

            return entity;
        }

        public Entity RecycleUnchecked()
        {
            var recycle = _denseArray[Length + _removed].Entity;
            var entity = new Entity(recycle.Id, recycle.Gen + 1);
            var components = _components[Length + _removed];

            _sparseArray[entity.Id] = Length;
            _components.Add(components);
            _denseArray.Add((entity, components.AsReadOnly()));
            _removed--;

            return entity;
        }

        public bool Contains(Entity entity)
        {
            Verifier.EntityNotNull(entity);
            return _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        IEntities IEntities.Remove(Entity entity)
        {
            Verifier.EntityExists(entity, this);
            return Remove(entity);
        }

        public Entities Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            var remove = _denseArray[index];

            foreach (var component in remove.Components)
            {
                _world.PoolsInternal.GetPoolUnchecked(component).RemoveUnchecked(entity);
                _world.FiltersInternal.EraseUnchecked(entity, component);
            }

            _components[index].Clear();

            _removed++;
            _sparseArray
                .Set(_denseArray[^1].Entity.Id, index)
                .Set(entity.Id, 0);
            _denseArray
                .RemoveAt(index)
                .Swap(Length, Length + _removed);
            _components
                .RemoveAt(index)
                .Swap(Length, Length + _removed);
            return this;
        }

        public Entities RecordUnchecked(Entity entity, Type component)
        {
            _components[_sparseArray[entity.Id]].Add(component);
            return this;
        }

        public Entities EraseUnchecked(Entity entity, Type component)
        {
            var components = _components[_sparseArray[entity.Id]];

            for (var i = components.Length - 1; i >= 0; i--)
            {
                if (component == components[i])
                {
                    components.RemoveAt(i);
                    return this;
                }
            }

            return this;
        }

        public ReadOnlySpan<(Entity Entity, ReadOnlyDenseArray<Type> Components)> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public IEnumerator<(Entity Entity, ReadOnlyDenseArray<Type> Components)> GetEnumerator()
        {
            return _denseArray.GetEnumerator();
        }
    }
}