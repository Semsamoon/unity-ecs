using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IEntities
    {
        public int Length { get; }

        public (Entity Entity, ReadOnlyDenseArray<Type> Components) this[int index] { get; }

        public Entity Create();
        public Entity Create(int componentsCapacity);
        public bool Contains(Entity entity);
        public IEntities Remove(Entity entity);

        public ReadOnlySpan<(Entity Entity, ReadOnlyDenseArray<Type> Components)> AsReadOnlySpan();
        public IEnumerator<(Entity Entity, ReadOnlyDenseArray<Type> Components)> GetEnumerator();
    }
}