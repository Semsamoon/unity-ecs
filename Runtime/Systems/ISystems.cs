namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Systems"/>.
    /// </summary>
    public interface ISystems
    {
        /// <summary>
        /// Creates and adds a new instance of the specified system type <typeparamref name="T"/>.
        /// </summary>
        public ISystems Add<T>() where T : ISystem, new();

        /// <summary>
        /// Adds the existing system instance.
        /// </summary>
        public ISystems Add(ISystem system);

        /// <summary>
        /// Updates all added systems.
        /// </summary>
        public ISystems Update();
    }
}