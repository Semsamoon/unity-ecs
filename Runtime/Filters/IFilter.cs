using System.Collections.Generic;

namespace ECS
{
    public interface IFilter
    {
        public bool Contains(Entity entity);

        public IEnumerator<Entity> GetEnumerator();
    }
}