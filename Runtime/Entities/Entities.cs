using System;

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

        public int Length => _denseArray.Length;

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
            return _removed > 0 ? Recycle() : CreateNew();
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

        private Entity CreateNew()
        {
            var entity = new Entity(++_id, 0);
            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            return entity;
        }

        private Entity Recycle()
        {
            while (_removed > 0)
            {
                _removed--;
                var recycle = _denseArray[Length + _removed];

                if (recycle.Gen >= int.MaxValue)
                {
                    continue;
                }

                recycle = new Entity(recycle.Id, recycle.Gen + 1);
                _denseArray.Add(recycle);
                return recycle;
            }

            return CreateNew();
        }
    }
}