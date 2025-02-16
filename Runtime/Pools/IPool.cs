using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Pool"/>.
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Gets the current number of tags.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the entity with the associated tag at the specified index.
        /// </summary>
        public Entity this[int index] { get; }

        /// <summary>
        /// Associates the tag with the specified entity.
        /// </summary>
        /// <remarks>
        /// <i>The tag must not already be associated with the entity.</i>
        /// </remarks>
        public IPool Add(Entity entity);

        /// <summary>
        /// Checks if the pool contains a tag associated with the specified entity.
        /// </summary>
        public bool Contains(Entity entity);

        /// <summary>
        /// Removes the tag associated with the specified entity.
        /// </summary>
        /// <remarks>
        /// <i>The tag must be associated with the entity.</i>
        /// </remarks>
        public IPool Remove(Entity entity);

        /// <summary>
        /// Converts the pool into a read-only span of entities with the associated tags.
        /// </summary>
        public ReadOnlySpan<Entity> AsReadOnlySpan();

        public IEnumerator<Entity> GetEnumerator();
    }

    /// <summary>
    /// Defines safe operations for the <see cref="Pool{T}"/>.
    /// </summary>
    public interface IPool<T>
    {
        /// <summary>
        /// Gets the current number of components.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the entity with the associated component at the specified index.
        /// </summary>
        public Entity this[int index] { get; }

        /// <summary>
        /// Associates the component with the specified entity.
        /// If the component is already associated, it will be overwritten.
        /// </summary>
        public IPool<T> Set(Entity entity, T value);

        /// <summary>
        /// Gets the component associated with the specified entity.
        /// If the component does not exist, it will be created and associated.
        /// </summary>
        public ref T Get(Entity entity);

        /// <summary>
        /// Gets the component associated with an entity at the specified index.
        /// </summary>
        public ref T Get(int index);

        /// <summary>
        /// Checks if the pool contains a component associated with the specified entity.
        /// </summary>
        public bool Contains(Entity entity);

        /// <summary>
        /// Removes the component associated with the specified entity.
        /// </summary>
        /// <remarks>
        /// <i>The component must be associated with the entity.</i>
        /// </remarks>
        public IPool<T> Remove(Entity entity);

        /// <summary>
        /// Converts the pool into a read-only span of entities with the associated components.
        /// </summary>
        public ReadOnlySpan<(Entity Entity, T Value)> AsReadOnlySpan();

        public IEnumerator<(Entity Entity, T Value)> GetEnumerator();
    }

    public interface IPoolInternal
    {
        public int Capacity { get; }
        public int Length { get; }

        public Entity this[int index] { get; }

        public bool Contains(Entity entity);
        public IPoolInternal RemoveUnchecked(Entity entity);
    }
}