using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class Filter
    {
        private readonly SparseArray<int> _counts;
        private readonly Pool _filtered;
        private readonly int _sum;

        public Entity this[int index] => _filtered[index];

        public Filter(DenseArray<IContains> included, DenseArray<IContains> excluded)
        {
            _filtered = new Pool();
            _counts = new SparseArray<int>();
            _sum = included.Length + excluded.Length;
        }

        public Filter(DenseArray<IContains> included, DenseArray<IContains> excluded, int sparseCapacity, int denseCapacity)
        {
            _filtered = new Pool(sparseCapacity, denseCapacity);
            _counts = new SparseArray<int>(sparseCapacity);
            _sum = included.Length + excluded.Length;
        }

        public void Recheck(Entity entity, DenseArray<IContains> included, DenseArray<IContains> excluded)
        {
            _counts[entity.Id] = 0;

            foreach (var pool in included)
            {
                if (pool.Contains(entity))
                {
                    _counts[entity.Id]++;
                }
            }

            foreach (var pool in excluded)
            {
                if (!pool.Contains(entity))
                {
                    _counts[entity.Id]++;
                }
            }

            if (_counts[entity.Id] == _sum)
            {
                _filtered.Add(entity);
            }
        }

        public void Change(Entity entity, int difference)
        {
            if (difference == 0)
            {
                return;
            }

            if (_counts[entity.Id] == _sum)
            {
                _counts[entity.Id] += difference;
                _filtered.Remove(entity);
                return;
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