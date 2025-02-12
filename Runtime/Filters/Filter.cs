using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Filter is a container that holds entities with
    /// specified components and tags included and excluded.
    /// </summary>
    public sealed class Filter : IFilter
    {
        private readonly SparseArray<int> _counts;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Sum { get; set; }

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Filter() : this(0, OptionsFilter.Default, OptionsEntities.Default)
        {
        }

        public Filter(int sum) : this(sum, OptionsFilter.Default, OptionsEntities.Default)
        {
        }

        public Filter(int sum, OptionsFilter filterOptions, OptionsEntities entitiesOptions)
        {
            _counts = new SparseArray<int>(entitiesOptions.Capacity);
            _sparseArray = new SparseArray<int>(entitiesOptions.Capacity);
            _denseArray = new DenseArray<Entity>(filterOptions.Capacity);
            Sum = sum;
        }

        public Filter ChangeUnchecked(Entity entity, int difference)
        {
            if (Contains(entity))
            {
                RemoveUnchecked(entity);
            }

            _counts[entity.Id] += difference;

            if (_counts[entity.Id] == Sum)
            {
                AddUnchecked(entity);
            }

            return this;
        }

        public Filter AddUnchecked(Entity entity)
        {
            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            return this;
        }

        public bool Contains(Entity entity)
        {
            Verifier.EntityNotNull(entity);
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public Filter RemoveUnchecked(Entity entity)
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
}