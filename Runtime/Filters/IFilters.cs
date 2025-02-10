namespace ECS
{
    public interface IFilters
    {
        public IFilterBuilderEmpty Create();
        public IFilterBuilderEmpty Create(OptionsFilterBuilder options);
    }
}