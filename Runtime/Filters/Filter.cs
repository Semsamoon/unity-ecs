using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Filter is a container that holds entities with
    /// specified components and tags included and excluded.
    /// </summary>
    public sealed class Filter
    {
        private readonly SparseArray<int> _counts;
        private readonly Pool _filtered;
        private readonly int _sum;

        public int Capacity => _filtered.Capacity;
        public int Length => _filtered.Length;

        public Entity this[int index] => _filtered[index];

        public Filter(int sum)
        {
            sum = Math.Max(0, sum);
            _filtered = new Pool();
            _counts = new SparseArray<int>();
            _sum = sum;
        }

        public Filter(int sum, int sparseCapacity, int denseCapacity)
        {
            sum = Math.Max(0, sum);
            _filtered = new Pool(sparseCapacity, denseCapacity);
            _counts = new SparseArray<int>(sparseCapacity);
            _sum = sum;
        }

        public void Change(Entity entity, int difference)
        {
            if (_counts[entity.Id] == _sum)
            {
                _filtered.Remove(entity);
            }

            _counts[entity.Id] += difference;

            if (_counts[entity.Id] == _sum)
            {
                _filtered.Add(entity);
            }
        }

        public bool Contains(Entity entity)
        {
            return _filtered.Contains(entity);
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _filtered.AsReadOnlySpan();
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _filtered.GetEnumerator();
        }
    }
}