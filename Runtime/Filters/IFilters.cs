namespace ECS
{
    public interface IFilters
    {
        public IFilterBuilderEmpty Create();
        public IFilterBuilderEmpty Create(int filterEntitiesCapacity);

        public IFilters IncludeCapacity<T>(int capacity);
        public IFilters ExcludeCapacity<T>(int capacity);
    }
}