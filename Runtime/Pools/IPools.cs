namespace ECS
{
    public interface IPools
    {
        public IPools Add<T>();
        public IPools Add<T>(in OptionsPool options);

        public IPool<T> Get<T>();
        public IPool<T> Get<T>(in OptionsPool options);
        public IPool GetTag<T>() where T : ITag;
        public IPool GetTag<T>(in OptionsPool options) where T : ITag;
    }
}