﻿using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Represents a collection of entities filtered based on included and excluded components.
    /// </summary>
    public sealed class Filter : IFilter
    {
        private readonly SparseArray<int> _counts;
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;

        public int Sum { get; set; }

        public int Capacity => _denseArray.Capacity;
        public int Length => _denseArray.Length;

        Entity IFilter.this[int index]
        {
            get
            {
                Verifier.ArgumentError(nameof(index), index < Length, $"must be less than Length {Length}.");
                return _denseArray[index];
            }
        }

        public Entity this[int index] => _denseArray[index];

        public Filter(
            int sum = 0,
            int entitiesCapacity = Options.DefaultEntitiesCapacity,
            int filterEntitiesCapacity = Options.DefaultFilterEntitiesCapacity)
        {
            Sum = sum;
            _counts = new SparseArray<int>(entitiesCapacity);
            _sparseArray = new SparseArray<int>(entitiesCapacity);
            _denseArray = new DenseArray<Entity>(filterEntitiesCapacity);
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

        bool IFilter.Contains(Entity entity)
        {
            Verifier.EntityNotNull(entity);
            return _denseArray[_sparseArray[entity.Id]] == entity;
        }

        public bool Contains(Entity entity)
        {
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