﻿using System.Collections.Generic;

namespace ECS
{
    public interface IPool
    {
        public void Add(Entity entity);
        public bool Contains(Entity entity);
        public void Remove(Entity entity);

        public IEnumerator<Entity> GetEnumerator();
    }

    public interface IPool<T>
    {
        public void Set(Entity entity, T value);
        public ref T Get(Entity entity);
        public bool Contains(Entity entity);
        public void Remove(Entity entity);

        public IEnumerator<(Entity Entity, T Value)> GetEnumerator();
    }

    public interface IPoolInternal
    {
        public int Capacity { get; }
        public int Length { get; }

        public bool Contains(Entity entity);
        public void Remove(Entity entity);
    }
}