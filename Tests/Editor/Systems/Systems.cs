using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Systems
    {
        [Test]
        public void Constructor()
        {
            var systems10 = new ECS.Systems(null, new OptionsSystems(10));
            var systems_10 = new ECS.Systems(null, new OptionsSystems(-10));
            var systems = new ECS.Systems(null);

            Assert.AreEqual(10, systems10.Capacity);
            Assert.Positive(systems_10.Capacity);
            Assert.Positive(systems.Capacity);

            Assert.AreEqual(0, systems10.Length);
            Assert.AreEqual(0, systems_10.Length);
            Assert.AreEqual(0, systems.Length);
        }

        [Test]
        public void Getter()
        {
            var systems = new ECS.Systems(null);
            var systemA = new ASystem();

            systems.Add(systemA);

            Assert.AreEqual(systemA, systems[0]);
            Assert.AreEqual(null, systems[1]);
        }

        [Test]
        public void Add()
        {
            var systems = new ECS.Systems(null);
            var systemA = new ASystem();

            systems
                .Add(systemA)
                .Add<BSystem>();

            Assert.AreEqual(2, systems.Length);
            Assert.AreEqual(systemA, systems[0]);
            Assert.IsInstanceOf<BSystem>(systems[1]);
            Assert.True(systemA.Initialized);
            Assert.True(((BSystem)systems[1]).Initialized);
        }

        [Test]
        public void Update()
        {
            var systems = new ECS.Systems(null);
            var systemA = new ASystem();

            systems
                .Add(systemA)
                .Add<BSystem>()
                .Update();

            Assert.AreEqual(1, systemA.UpdateCount);
            Assert.AreEqual(1, ((BSystem)systems[1]).UpdateCount);

            systems.Update();

            Assert.AreEqual(2, systemA.UpdateCount);
            Assert.AreEqual(2, ((BSystem)systems[1]).UpdateCount);
        }

        [Test]
        public void Destroy()
        {
            var systems = new ECS.Systems(null);
            var systemA = new ASystem();

            systems
                .Add(systemA)
                .Add<BSystem>()
                .Destroy();

            Assert.True(systemA.Destroyed);
            Assert.True(((BSystem)systems[1]).Destroyed);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var systems = new ECS.Systems(null);

            for (var i = 0; i < 4; i++)
            {
                systems.Add<ASystem>();
            }

            var span = systems.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.IsInstanceOf<ASystem>(span[i]);
                Assert.AreEqual(systems[i], span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var systems = new ECS.Systems(null);

            for (var i = 0; i < 4; i++)
            {
                systems.Add<ASystem>();
            }

            var j = 0;
            foreach (var system in systems)
            {
                Assert.IsInstanceOf<ASystem>(system);
                j++;
            }

            Assert.AreEqual(4, j);
        }

        private class ASystem : ISystem
        {
            public bool Initialized { get; private set; }
            public int UpdateCount { get; private set; }
            public bool Destroyed { get; private set; }

            public void Initialize(IWorld world)
            {
                Initialized = true;
            }

            public void Update()
            {
                UpdateCount++;
            }

            public void Destroy()
            {
                Destroyed = true;
            }
        }

        private class BSystem : ISystem
        {
            public bool Initialized { get; private set; }
            public int UpdateCount { get; private set; }
            public bool Destroyed { get; private set; }

            public void Initialize(IWorld world)
            {
                Initialized = true;
            }

            public void Update()
            {
                UpdateCount++;
            }

            public void Destroy()
            {
                Destroyed = true;
            }
        }
    }
}