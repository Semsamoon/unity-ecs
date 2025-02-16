using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Filter"/>.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Gets the number of entities.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the entity at the specified index.
        /// </summary>
        public Entity this[int index] { get; }

        /// <summary>
        /// Checks if the filter contains the specified entity.
        /// </summary>
        public bool Contains(Entity entity);

        /// <summary>
        /// Converts the entity collection into a read-only span of filtered entities.
        /// </summary>
        public ReadOnlySpan<Entity> AsReadOnlySpan();

        public IEnumerator<Entity> GetEnumerator();
    }
}