namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="World"/>.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// Gets the <see cref="Entities"/>.
        /// </summary>
        public IEntities Entities { get; }

        /// <summary>
        /// Gets the <see cref="Pools"/>.
        /// </summary>
        public IPools Pools { get; }

        /// <summary>
        /// Gets the <see cref="Filters"/>.
        /// </summary>
        public IFilters Filters { get; }

        /// <summary>
        /// Gets the <see cref="Systems"/>.
        /// </summary>
        public ISystems Systems { get; }

        /// <summary>
        /// Performs cleanup and destroys the world.
        /// </summary>
        public void Destroy();
    }
}