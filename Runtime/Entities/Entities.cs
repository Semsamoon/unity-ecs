using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Entities is a manager for entities.
    /// </summary>
    public sealed class Entities
    {
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;
        private int _removed;
        private int _id;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Entities()
        {
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<Entity>();
        }

        public Entities(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
        }

        public Entity Create()
        {
            if (_removed > 0)
            {
                _removed--;
                var removed = _denseArray[Length + _removed];
                var recycled = new Entity(removed.Id, removed.Gen + 1);
                _sparseArray[recycled.Id] = Length;
                _denseArray.Add(recycled);
                return recycled;
            }

            _id++;
            var created = new Entity(_id, 0);
            _sparseArray[created.Id] = Length;
            _denseArray.Add(created);
            return created;
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
            _denseArray.RemoveAt(index);
            _removed++;
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
}