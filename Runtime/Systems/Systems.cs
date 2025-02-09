using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Systems is a manager for systems.
    /// </summary>
    public sealed class Systems : ISystems
    {
        private readonly World _world;
        private readonly DenseArray<ISystem> _systems;

        public int Capacity => _systems.Capacity;
        public int Length => _systems.Length;

        public ISystem this[int index] => _systems[index];

        public Systems(World world)
        {
            _world = world;
            _systems = new DenseArray<ISystem>(OptionsSystems.DefaultCapacity);
        }

        public Systems(World world, OptionsSystems options)
        {
            options = options.Validate();
            _world = world;
            _systems = new DenseArray<ISystem>(options.Capacity);
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

        public ReadOnlySpan<ISystem> AsReadOnlySpan()
        {
            return _systems.AsReadOnlySpan();
        }

        public IEnumerator<ISystem> GetEnumerator()
        {
            return _systems.GetEnumerator();
        }
    }
}