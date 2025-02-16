namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Pools"/>.
    /// </summary>
    public interface IPools
    {
        /// <summary>
        /// Adds a new pool for the specified type <typeparamref name="T"/>.
        /// </summary>
        public IPools Add<T>();

        /// <summary>
        /// Adds a new pool for the specified type <typeparamref name="T"/> with the specified pool capacity.
        /// </summary>
        public IPools Add<T>(int poolComponentsCapacity);

        /// <summary>
        /// Gets the component pool of the specified type <typeparamref name="T"/>.
        /// If the pool does not exist, it will be created and added.
        /// </summary>
        /// <remarks>
        /// <i>The type must be a component type.</i>
        /// </remarks>
        public IPool<T> Get<T>();

        /// <summary>
        /// Gets the component pool of the specified type <typeparamref name="T"/>.
        /// If the pool does not exist, it will be created with the specified pool capacity and added.
        /// </summary>
        /// <remarks>
        /// <i>The type must be a component type.</i>
        /// </remarks>
        public IPool<T> Get<T>(int poolComponentsCapacity);

        /// <summary>
        /// Gets the tag pool of the specified type <typeparamref name="T"/>.
        /// If the pool does not exist, it will be created and added.
        /// </summary>
        /// <remarks>
        /// <i>The type must be a tag type.</i>
        /// </remarks>
        public IPool GetTag<T>() where T : ITag;

        /// <summary>
        /// Gets the tag pool of the specified type <typeparamref name="T"/>.
        /// If the pool does not exist, it will be created with the specified pool capacity and added.
        /// </summary>
        /// <remarks>
        /// <i>The type must be a tag type.</i>
        /// </remarks>
        public IPool GetTag<T>(int poolComponentsCapacity) where T : ITag;
    }
}