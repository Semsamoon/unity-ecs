namespace ECS
{
    public interface IPools
    {
        public IPools Add<T>();
        public IPools Add<T>(OptionsPool options);

        public IPool<T> Get<T>();
        public IPool<T> Get<T>(OptionsPool options);
        public IPool GetTag<T>() where T : ITag;
        public IPool GetTag<T>(OptionsPool options) where T : ITag;
    }
}