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
        private readonly DenseArray<(Entity Entity, DenseArray<Type> Components)> _denseArray;
        private readonly int _defaultComponentsCapacity;

        private int _removed;
        private int _id;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public (Entity Entity, DenseArray<Type> Components) this[int index] => _denseArray[index];

        public Entities(World world) : this(world, OptionsEntities.Default())
        {
        }

        public Entities(World world, OptionsEntities options)
        {
            options = options.Validate();
            _world = world;
            _sparseArray = new SparseArray<int>(options.Capacity);
            _denseArray = new DenseArray<(Entity, DenseArray<Type>)>(options.Capacity);
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
            _denseArray[_sparseArray[recycle.Id]].Components.ExtendTo(componentsCapacity);
            return recycle;
        }

        public Entity CreateUnchecked(int componentsCapacity)
        {
            var entity = new Entity(_id, 1);

            _sparseArray[_id] = Length;
            _denseArray.Add((entity, new DenseArray<Type>(componentsCapacity)));
            _id++;

            return entity;
        }

        public Entity RecycleUnchecked()
        {
            var recycle = _denseArray[Length + _removed];
            var entity = new Entity(recycle.Entity.Id, recycle.Entity.Gen + 1);

            _sparseArray[entity.Id] = Length;
            _denseArray.Add((entity, recycle.Components));
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

            remove.Components.Clear();

            _removed++;
            _sparseArray
                .Set(_denseArray[^1].Entity.Id, index)
                .Set(entity.Id, 0);
            _denseArray
                .RemoveAt(index)
                .Swap(Length, Length + _removed);
            return this;
        }

        public Entities RecordUnchecked(Entity entity, Type component)
        {
            _denseArray[_sparseArray[entity.Id]].Components.Add(component);
            return this;
        }

        public Entities EraseUnchecked(Entity entity, Type component)
        {
            var components = _denseArray[_sparseArray[entity.Id]].Components;

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

        public ReadOnlySpan<(Entity Entity, DenseArray<Type> Components)> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public IEnumerator<(Entity Entity, DenseArray<Type> Components)> GetEnumerator()
        {
            return _denseArray.GetEnumerator();
        }
    }
}