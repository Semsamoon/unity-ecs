namespace ECS
{
    public interface IPools
    {
        public IPools Add<T>();
        public IPools Add<T>(int poolComponentsCapacity);

        public IPool<T> Get<T>();
        public IPool<T> Get<T>(int poolComponentsCapacity);
        public IPool GetTag<T>() where T : ITag;
        public IPool GetTag<T>(int poolComponentsCapacity) where T : ITag;
    }
}