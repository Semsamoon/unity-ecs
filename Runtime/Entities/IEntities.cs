using System.Collections.Generic;

namespace ECS
{
    public interface IEntities
    {
        public Entity Create();
        public bool Contains(Entity entity);
        public void Remove(Entity entity);

        public IEnumerator<Entity> GetEnumerator();
    }
}