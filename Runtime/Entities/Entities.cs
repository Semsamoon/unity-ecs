using System;

namespace ECS
{
    /// <summary>
    /// An entities' storage. Consists of one <see cref="Pool"/>
    /// with existing entities and another with removed entities.
    /// Also holds the smallest unused <see cref="Entity.Id"/>
    /// that is incremented each time a new entity is created.<br/>
    /// <br/>
    /// <i>An entities' storage is used to control existing and
    /// removed entities so that they are always valid.</i>
    /// </summary>
    public sealed class Entities
    {
        private readonly Pool _pool;
        private readonly Pool _removed;
        private int _id;

        /// <summary>
        /// Current length of internal <see cref="Pool"/> with existing entities.
        /// </summary>
        public int Length => _pool.Length;

        public Entities()
        {
            _pool = new Pool();
            _removed = new Pool();
            _id = 1;
        }

        /// <param name="sparseCapacity">Initial sparse capacity of internal <see cref="Pool"/></param>
        /// <param name="denseCapacity">Initial dense capacity of internal <see cref="Pool"/></param>
        public Entities(int sparseCapacity, int denseCapacity)
        {
            _pool = new Pool(sparseCapacity, denseCapacity);
            _removed = new Pool(sparseCapacity, denseCapacity);
            _id = 1;
        }

        /// <returns>Created valid <see cref="Entity"/></returns>
        public Entity Create()
        {
            return _removed.Length <= 1 ? CreateNew() : Recycle();
        }

        /// <returns>True if the entity exists, false elsewhere</returns>
        public bool Contains(Entity entity)
        {
            return _pool.Contains(entity);
        }

        /// <summary>
        /// Removes the <paramref name="entity"/> from internal <see cref="Pool"/>
        /// with existing entities and adds it to Pool with removed entities.<br/>
        /// <br/>
        /// <i>Removed <see cref="Entity"/> must not be used anywhere anymore.</i>
        /// </summary>
        public void Remove(Entity entity)
        {
            if (!_pool.Contains(entity))
            {
                return;
            }

            _removed.Add(entity);
            _pool.Remove(entity);
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _pool.AsReadOnlySpan();
        }

        /// <returns>A new entity with incremented <see cref="Entity.Id"/></returns>
        private Entity CreateNew()
        {
            var entity = new Entity(_id++, 0);
            _pool.Add(entity);
            return entity;
        }

        /// <returns>
        /// The old entity with incremented <see cref="Entity.Gen"/>.
        /// If it is not possible, then calls <see cref="CreateNew"/>
        /// </returns>
        private Entity Recycle()
        {
            while (_removed.Length > 1)
            {
                var recycle = _removed[1];
                _removed.Remove(recycle);

                if (recycle.Gen >= int.MaxValue)
                {
                    continue;
                }

                recycle = new Entity(recycle.Id, recycle.Gen + 1);
                _pool.Add(recycle);
                return recycle;
            }

            return CreateNew();
        }
    }
}