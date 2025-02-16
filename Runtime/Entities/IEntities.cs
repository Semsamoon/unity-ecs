using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Entities"/>.
    /// </summary>
    public interface IEntities
    {
        /// <summary>
        /// Gets the current number of entities.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the entity and its associated component types at the specified index.
        /// </summary>
        public (Entity Entity, ReadOnlyDenseArray<Type> Components) this[int index] { get; }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public Entity Create();

        /// <summary>
        /// Creates a new entity with the specified component types capacity.
        /// </summary>
        public Entity Create(int componentsCapacity);

        /// <summary>
        /// Checks if the entity collection contains the specified entity.
        /// </summary>
        public bool Contains(Entity entity);

        /// <summary>
        /// Removes the specified entity with its components.
        /// </summary>
        public IEntities Remove(Entity entity);

        /// <summary>
        /// Converts the entity collection into a read-only span of entities and their component types.
        /// </summary>
        public ReadOnlySpan<(Entity Entity, ReadOnlyDenseArray<Type> Components)> AsReadOnlySpan();

        public IEnumerator<(Entity Entity, ReadOnlyDenseArray<Type> Components)> GetEnumerator();
    }
}