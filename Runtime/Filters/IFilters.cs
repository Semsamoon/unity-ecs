namespace ECS
{
    public interface IFilters
    {
        public IFilterBuilderEmpty Create();
        public IFilterBuilderEmpty Create(OptionsFilter options);

        public IFilters IncludeCapacity<T>(int capacity);
        public IFilters ExcludeCapacity<T>(int capacity);
    }
}