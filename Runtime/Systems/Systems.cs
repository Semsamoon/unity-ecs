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

        public Systems(World world) : this(world, OptionsSystems.Default)
        {
        }

        public Systems(World world, OptionsSystems options)
        {
            _world = world;
            _systems = new DenseArray<ISystem>(options.Capacity);
        }

        ISystems ISystems.Add<T>()
        {
            return Add(new T());
        }

        public Systems Add<T>() where T : ISystem, new()
        {
            return Add(new T());
        }

        ISystems ISystems.Add(ISystem system)
        {
            return Add(system);
        }

        public Systems Add(ISystem system)
        {
            _systems.Add(system);
            system.Initialize(_world);
            return this;
        }

        ISystems ISystems.Update()
        {
            return Update();
        }

        public Systems Update()
        {
            foreach (var system in _systems)
            {
                system.Update();
            }

            return this;
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