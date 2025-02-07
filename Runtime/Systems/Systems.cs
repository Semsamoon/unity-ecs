namespace ECS
{
    public sealed class Systems : ISystems
    {
        private readonly World _world;
        private readonly DenseArray<ISystem> _systems;

        public Systems(World world)
        {
            _world = world;
            _systems = new DenseArray<ISystem>();
        }

        public Systems(World world, int capacity)
        {
            _world = world;
            _systems = new DenseArray<ISystem>(capacity);
        }

        public ISystems Add<T>() where T : ISystem, new()
        {
            var system = new T();
            _systems.Add(system);
            system.Initialize(_world);
            return this;
        }

        public ISystems Add(ISystem system)
        {
            _systems.Add(system);
            system.Initialize(_world);
            return this;
        }

        public void Update()
        {
            foreach (var system in _systems)
            {
                system.Update();
            }
        }

        public void Destroy()
        {
            foreach (var system in _systems)
            {
                system.Destroy();
            }
        }
    }
}