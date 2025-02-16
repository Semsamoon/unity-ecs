namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="Filters"/>.
    /// </summary>
    public interface IFilters
    {
        /// <summary>
        /// Creates a new filter.
        /// </summary>
        public IFilterBuilderEmpty Create();

        /// <summary>
        /// Creates a new filter with the specified filter capacity.
        /// </summary>
        public IFilterBuilderEmpty Create(int filterEntitiesCapacity);

        /// <summary>
        /// Sets the specified capacity for internal array of filters with included component of type <typeparamref name="T"/>.
        /// If the internal array does not exist, it will be created.
        /// </summary>
        public IFilters IncludeCapacity<T>(int capacity);

        /// <summary>
        /// Sets the specified capacity for internal array of filters with excluded component of type <typeparamref name="T"/>.
        /// If the internal array does not exist, it will be created.
        /// </summary>
        public IFilters ExcludeCapacity<T>(int capacity);
    }
}