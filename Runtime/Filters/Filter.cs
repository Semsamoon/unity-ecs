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
        private readonly int _sum;

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        public Entity this[int index] => _denseArray[index];

        public Filter(int sum) : this(sum, OptionsFilter.Default())
        {
        }

        public Filter(int sum, OptionsFilter options)
        {
            sum = Math.Max(0, sum);
            options = options.Validate();
            _counts = new SparseArray<int>(options.EntitiesCapacity);
            _sparseArray = new SparseArray<int>(options.EntitiesCapacity);
            _denseArray = new DenseArray<Entity>(options.Capacity);
            _sum = sum;
        }

        public void Change(Entity entity, int difference)
        {
            if (entity.IsNull())
            {
                return;
            }

            if (_counts[entity.Id] == _sum && Contains(entity))
            {
                Remove(entity);
            }

            _counts[entity.Id] += difference;

            if (_counts[entity.Id] == _sum)
            {
                Add(entity);
            }
        }

        public bool Contains(Entity entity)
        {
            return !entity.IsNull() && _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _denseArray.GetEnumerator();
        }

        private void Add(Entity entity)
        {
            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
        }

        private void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];
            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index] = new Entity();
            _denseArray.RemoveAt(index);
        }
    }
}