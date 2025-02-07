namespace ECS
{
    public interface IPools
    {
        public IPools Add<T>();
        public IPools Add<T>(int sparseCapacity, int denseCapacity);

        public IPool<T> Get<T>();
        public IPool<T> Get<T>(int sparseCapacity, int denseCapacity);
        public IPool GetTag<T>() where T : ITag;
        public IPool GetTag<T>(int sparseCapacity, int denseCapacity) where T : ITag;
    }
}