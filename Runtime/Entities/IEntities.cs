using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IEntities
    {
        public Entity Create();
        public Entity Create(int componentsCapacity);
        public bool Contains(Entity entity);
        public IEntities Remove(Entity entity);

        public ReadOnlySpan<(Entity Entity, DenseArray<Type> Components)> AsReadOnlySpan();
        public IEnumerator<(Entity Entity, DenseArray<Type> Components)> GetEnumerator();
    }
}