using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Entities is a manager for entities.
    /// </summary>
    public sealed class Entities : IEntities
    {
        private const int DefaultComponentsCapacity = 4;

        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<(Entity Entity, DenseArray<Type> Components)> _denseArray;
        private int _removed;
        private int _id;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public (Entity Entity, DenseArray<Type> Components) this[int index] => _denseArray[index];

        public Entities()
        {
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<(Entity, DenseArray<Type>)>();
        }

        public Entities(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<(Entity, DenseArray<Type>)>(denseCapacity);
        }

        public Entity Create()
        {
            if (_removed > 0)
            {
                var removed = _denseArray[Length + _removed];
                var recycled = new Entity(removed.Entity.Id, removed.Entity.Gen + 1);
                _sparseArray[recycled.Id] = Length;
                _denseArray.Add((recycled, removed.Components));
                _removed--;
                return recycled;
            }

            _id++;
            var created = new Entity(_id, 0);
            _sparseArray[created.Id] = Length;
            _denseArray.Add((created, new DenseArray<Type>(DefaultComponentsCapacity)));
            return created;
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

            for (var i = _denseArray[index].Components.Length; i >= 0; i--)
            {
                _denseArray[index].Components.RemoveAt(i);
            }

            _sparseArray[_denseArray[^1].Entity.Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray.RemoveAt(index);
            _removed++;
            (_denseArray[Length], _denseArray[Length + _removed]) = (_denseArray[Length + _removed], _denseArray[Length]);
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