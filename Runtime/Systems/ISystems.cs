namespace ECS
{
    public interface ISystems
    {
        public ISystems Add<T>() where T : ISystem, new();
        public ISystems Add(ISystem system);

        public ISystems Update();
    }
}