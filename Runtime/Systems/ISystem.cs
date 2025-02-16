namespace ECS
{
    /// <summary>
    /// Represents an interface for systems.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Initializes the system with the specified <see cref="World"/>.
        /// </summary>
        public void Initialize(IWorld world);

        /// <summary>
        /// Updates the system's logic.
        /// </summary>
        public void Update();

        /// <summary>
        /// Performs cleanup when the system is destroyed.
        /// </summary>
        public void Destroy();
    }
}