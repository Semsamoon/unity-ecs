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
        private readonly SparseArray<int> _sparseArray;
        private readonly DenseArray<Entity> _denseArray;
        private int _removed;
        private int _id;

        /// <summary>
        /// Current length of internal <see cref="Pool"/> with existing entities.
        /// </summary>
        public int Length => _denseArray.Length;

        public Entities()
        {
            _sparseArray = new SparseArray<int>();
            _denseArray = new DenseArray<Entity>();
        }

        /// <param name="sparseCapacity">Initial sparse capacity of internal <see cref="Pool"/> with existing entities</param>
        /// <param name="denseCapacity">Initial dense capacity of internal <see cref="Pool"/> with existing entities</param>
        public Entities(int sparseCapacity, int denseCapacity)
        {
            _sparseArray = new SparseArray<int>(sparseCapacity);
            _denseArray = new DenseArray<Entity>(denseCapacity);
        }

        /// <returns>Created valid <see cref="Entity"/></returns>
        public Entity Create()
        {
            return _removed > 0 ? Recycle() : CreateNew();
        }

        /// <returns>True if the entity exists, false elsewhere</returns>
        public bool Contains(Entity entity)
        {
            return !entity.IsNull() && _denseArray[_sparseArray[entity.Id]] == entity;
        }

        /// <summary>
        /// Removes the <paramref name="entity"/> from internal <see cref="Pool"/>
        /// with existing entities and adds it to Pool with removed entities.<br/>
        /// <br/>
        /// <i>Removed <see cref="Entity"/> must not be used anywhere anymore.</i>
        /// </summary>
        public void Remove(Entity entity)
        {
            var index = _sparseArray[entity.Id];

            if (entity.IsNull() || _denseArray[index] != entity)
            {
                return;
            }

            _sparseArray[_denseArray[^1].Id] = index;
            _sparseArray[entity.Id] = 0;
            _denseArray.RemoveAt(index);
            _removed++;
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _denseArray.AsReadOnlySpan();
        }

        /// <returns>A new entity with incremented <see cref="Entity.Id"/></returns>
        private Entity CreateNew()
        {
            var entity = new Entity(++_id, 0);
            _sparseArray[entity.Id] = Length;
            _denseArray.Add(entity);
            return entity;
        }

        /// <returns>
        /// The old entity with incremented <see cref="Entity.Gen"/>.
        /// If it is not possible, then calls <see cref="CreateNew"/>
        /// </returns>
        private Entity Recycle()
        {
            while (_removed > 0)
            {
                _removed--;
                var recycle = _denseArray[Length + _removed];

                if (recycle.Gen >= int.MaxValue)
                {
                    continue;
                }

                recycle = new Entity(recycle.Id, recycle.Gen + 1);
                _denseArray.Add(recycle);
                return recycle;
            }

            return CreateNew();
        }
    }
}