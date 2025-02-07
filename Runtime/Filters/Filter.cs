﻿using System;
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

        public Filter(int sum)
        {
            sum = Math.Max(0, sum);
            _counts = new SparseArray<int>();
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<Entity>();
            _sum = sum;
        }

        public Filter(int sum, int sparseCapacity, int denseCapacity)
        {
            sum = Math.Max(0, sum);
            _counts = new SparseArray<int>(sparseCapacity);
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
            _sum = sum;
        }

        public void Change(Entity entity, int difference)
        {
            if (_counts[entity.Id] == _sum)
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
            if (entity.IsNull() || _denseArray[_sparseArray[entity.Id]] == entity)
            {
                return;
            }

            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
        }

        private void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity.IsNull() || _denseArray[index] != entity)
            {
                return;
            }

            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray[index] = new Entity();
            _denseArray.RemoveAt(index);
        }
    }
}