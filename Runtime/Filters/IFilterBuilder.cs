namespace ECS
{
    public interface IFilterBuilder : IFilterBuilderEmpty
    {
        public IFilter Build();
        public IFilter Build(OptionsFilter options);
    }

    public interface IFilterBuilderEmpty
    {
        public IFilterBuilder Include<T>();
        public IFilterBuilder Include<T1, T2>();
        public IFilterBuilder Include<T1, T2, T3>();

        public IFilterBuilder Exclude<T>();
        public IFilterBuilder Exclude<T1, T2>();
        public IFilterBuilder Exclude<T1, T2, T3>();
    }
}