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
            return Create(_defaultComponentsCapacity);
        }

        public Entity Create(int componentsCapacity)
        {
            if (_removed > 0)
            {
                var removed = _denseArray[Length + _removed];
                var recycled = new Entity(removed.Entity.Id, removed.Entity.Gen + 1);
                removed.Components.ExtendTo(componentsCapacity);
                _sparseArray[recycled.Id] = Length;
                _denseArray.Add((recycled, removed.Components));
                _removed--;
                return recycled;
            }

            var created = new Entity(_id, 1);
            _sparseArray[created.Id] = Length;
            _denseArray.Add((created, new DenseArray<Type>(componentsCapacity)));
            _id++;
            return created;
        }

        public bool Contains(Entity entity)
        {
            return entity != Entity.Null && _denseArray[_sparseArray[entity.Id]].Entity == entity;
        }

        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            var tuple = _denseArray[index];

            if (entity == Entity.Null || tuple.Entity != entity)
            {
                return;
            }

            foreach (var component in tuple.Components)
            {
                _world.PoolsInternal.GetPoolUnchecked(component).RemoveUnchecked(entity);
                _world.FiltersInternal.EraseUnchecked(entity, component);
            }

            tuple.Components.Clear();

            _sparseArray[_denseArray[^1].Entity.Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray.RemoveAt(index);
            _removed++;
            (_denseArray[Length], _denseArray[Length + _removed]) = (_denseArray[Length + _removed], _denseArray[Length]);
        }

        public void RecordUnchecked(Entity entity, Type component)
        {
            _denseArray[_sparseArray[entity.Id]].Components.Add(component);
        }

        public void EraseUnchecked(Entity entity, Type component)
        {
            var components = _denseArray[_sparseArray[entity.Id]].Components;

            for (var i = components.Length - 1; i >= 0; i++)
            {
                if (component == components[i])
                {
                    components.RemoveAt(i);
                    return;
                }
            }
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