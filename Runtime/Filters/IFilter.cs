using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IFilter
    {
        public int Length { get; }

        public Entity this[int index] { get; }

        public bool Contains(Entity entity);

        public ReadOnlySpan<Entity> AsReadOnlySpan();
        public IEnumerator<Entity> GetEnumerator();
    }
}