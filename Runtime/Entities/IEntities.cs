using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IEntities
    {
        public Entity Create();
        public Entity Create(int componentsCapacity);
        public bool Contains(Entity entity);
        public void Remove(Entity entity);

        public IEnumerator<(Entity Entity, DenseArray<Type> Components)> GetEnumerator();
    }
}