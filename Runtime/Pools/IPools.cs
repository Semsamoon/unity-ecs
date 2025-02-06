namespace ECS
{
    public interface IPools
    {
        public IPools Add<T>();
        public IPools Add<T>(int sparseCapacity, int denseCapacity);

        public Pool<T> Get<T>();
        public Pool<T> Get<T>(int sparseCapacity, int denseCapacity);
        public Pool GetTag<T>() where T : ITag;
        public Pool GetTag<T>(int sparseCapacity, int denseCapacity) where T : ITag;
    }
}