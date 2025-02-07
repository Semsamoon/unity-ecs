namespace ECS
{
    public interface IFilters
    {
        public FilterBuilder Create();
        public FilterBuilder Create(int included, int excluded);
    }
}