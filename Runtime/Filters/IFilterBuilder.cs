namespace ECS
{
    /// <summary>
    /// Defines safe operations for the <see cref="FilterBuilder"/> when it can be built.
    /// </summary>
    public interface IFilterBuilder : IFilterBuilderEmpty
    {
        /// <summary>
        /// Finishes building and returns the <see cref="IFilter"/>.
        /// </summary>
        public IFilter Build();
    }

    /// <summary>
    /// Defines safe operations for the <see cref="FilterBuilder"/> before any type have been included or excluded.
    /// </summary>
    public interface IFilterBuilderEmpty
    {
        /// <summary>
        /// Includes the specified type <typeparamref name="T"/> in the filter.
        /// </summary>
        public IFilterBuilder Include<T>();

        /// <summary>
        /// Includes the specified types <typeparamref name="T1"/> and <typeparamref name="T2"/> in the filter.
        /// </summary>
        public IFilterBuilder Include<T1, T2>();

        /// <summary>
        /// Includes the specified types <typeparamref name="T1"/>, <typeparamref name="T2"/> and <typeparamref name="T3"/> in the filter.
        /// </summary>
        public IFilterBuilder Include<T1, T2, T3>();

        /// <summary>
        /// Excludes the specified type <typeparamref name="T"/> from the filter.
        /// </summary>
        public IFilterBuilder Exclude<T>();

        /// <summary>
        /// Excludes the specified types <typeparamref name="T1"/> and <typeparamref name="T2"/> from the filter.
        /// </summary>
        public IFilterBuilder Exclude<T1, T2>();

        /// <summary>
        /// Excludes the specified types <typeparamref name="T1"/>, <typeparamref name="T2"/> and <typeparamref name="T3"/> from the filter.
        /// </summary>
        public IFilterBuilder Exclude<T1, T2, T3>();
    }
}