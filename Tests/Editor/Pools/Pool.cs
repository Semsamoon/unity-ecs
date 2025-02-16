using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Pool
    {
        [Test]
        public void Constructor()
        {
            var pool = new ECS.Pool(null, typeof(int));
            var pool10x10 = new ECS.Pool(null, typeof(int), new OptionsPool(10), OptionsEntities.Default);

            Assert.AreEqual(10, pool10x10.Capacity);
            Assert.Positive(pool.Capacity);

            Assert.AreEqual(0, pool.Length);
            Assert.AreEqual(0, pool10x10.Length);
        }

        [Test]
        public void Getter()
        {
            var world = (World)World.Create();
            var pool = world.Pools.GetTag<ATag>();
            var entity = new ECS.Entity();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool
                .Add(entity0x1)
                .Add(entity1x1);

            Assert.AreEqual(entity0x1, pool[0]);
            Assert.AreEqual(entity1x1, pool[1]);
            Assert.AreEqual(entity, pool[2]);
        }

        [Test]
        public void Add()
        {
            var world = (World)World.Create();
            var pool = world.Pools.GetTag<ATag>();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool.Add(entity0x1);

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(entity0x1, pool[0]);

            pool.AddUnchecked(entity1x1);

            Assert.AreEqual(2, pool.Length);
            Assert.AreEqual(entity1x1, pool[1]);
        }

        [Test]
        public void Contains()
        {
            var world = (World)World.Create();
            var pool = world.Pools.GetTag<ATag>();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool.Add(entity0x1);

            Assert.True(pool.Contains(entity0x1));
            Assert.False(pool.Contains(entity1x1));
        }

        [Test]
        public void Extending()
        {
            var world = (World)World.Create();
            var pool2x2 = world.Pools.GetTag<ATag>(new OptionsPool(2));

            for (var i = 0; i < 33; i++)
            {
                pool2x2.Add(world.Entities.Create());
            }

            Assert.AreEqual(64, pool2x2.Capacity);
            Assert.AreEqual(new ECS.Entity(32, 1), pool2x2[32]);
            Assert.AreEqual(new ECS.Entity(), pool2x2[33]);
        }

        [Test]
        public void Remove()
        {
            var world = (World)World.Create();
            var pool = world.Pools.GetTag<ATag>();
            var entity0x1 = world.Entities.Create();

            pool
                .Add(entity0x1)
                .Remove(entity0x1);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity0x1));

            pool
                .Add(entity0x1)
                .RemoveUnchecked(entity0x1);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity0x1));
        }

        [Test]
        public void ReadOnlySpan()
        {
            var world = (World)World.Create();
            var pool = new ECS.Pool(world, typeof(int));

            for (var i = 0; i < 4; i++)
            {
                pool.Add(world.Entities.Create());
            }

            var span = pool.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(new ECS.Entity(i, 1), span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var world = (World)World.Create();
            var pool = new ECS.Pool(world, typeof(int));

            for (var i = 0; i < 4; i++)
            {
                pool.Add(world.Entities.Create());
            }

            var j = 0;
            foreach (var value in pool)
            {
                Assert.AreEqual(new ECS.Entity(j, 1), value);
                j++;
            }

            Assert.AreEqual(4, j);
        }

        private struct ATag : ITag
        {
        }
    }

    public sealed class Pool_T
    {
        [Test]
        public void Constructor()
        {
            var pool = new Pool<int>(null);
            var pool10x10 = new Pool<int>(null, new OptionsPool(10), OptionsEntities.Default);

            Assert.AreEqual(10, pool10x10.Capacity);
            Assert.Positive(pool.Capacity);

            Assert.AreEqual(0, pool.Length);
            Assert.AreEqual(0, pool10x10.Length);
        }

        [Test]
        public void Getter()
        {
            var world = (World)World.Create();
            var pool = world.Pools.Get<int>();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();
            var entity2x1 = world.Entities.Create();

            pool.Get(entity0x1) = 10;
            pool.Get(entity1x1) = 20;

            Assert.AreEqual(entity0x1, pool[0]);
            Assert.AreEqual(entity1x1, pool[1]);
            Assert.AreEqual(10, pool.Get(0));
            Assert.AreEqual(20, pool.GetUnchecked(entity1x1));
            Assert.AreEqual(0, pool.Get(entity2x1));
        }

        [Test]
        public void Set()
        {
            var world = (World)World.Create();
            var pool = world.Pools.Get<int>();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool
                .Set(entity0x1, 10)
                .SetUnchecked(entity0x1, 20)
                .Set(entity1x1, 30);

            Assert.AreEqual(entity0x1, pool[0]);
            Assert.AreEqual(20, pool.Get(0));
            Assert.AreEqual(entity1x1, pool[1]);
            Assert.AreEqual(30, pool.Get(1));
        }

        [Test]
        public void Contains()
        {
            var world = (World)World.Create();
            var pool = world.Pools.Get<int>();
            var entity0x1 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool.Get(entity0x1) = 10;

            Assert.True(pool.Contains(entity0x1));
            Assert.False(pool.Contains(entity1x1));
        }

        [Test]
        public void Extending()
        {
            var world = (World)World.Create();
            var pool2x2 = world.Pools.Get<int>(new OptionsPool(2));

            for (var i = 0; i < 33; i++)
            {
                pool2x2.Get(world.Entities.Create()) = i + 1;
            }

            Assert.AreEqual(64, pool2x2.Capacity);
            Assert.AreEqual(new ECS.Entity(32, 1), pool2x2[32]);
            Assert.AreEqual(33, pool2x2.Get(32));
            Assert.AreEqual(new ECS.Entity(), pool2x2[33]);
            Assert.AreEqual(0, pool2x2.Get(33));
        }

        [Test]
        public void Remove()
        {
            var world = (World)World.Create();
            var pool = world.Pools.Get<int>();
            var entity0x1 = world.Entities.Create();

            pool
                .Set(entity0x1, 10)
                .Remove(entity0x1);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity0x1));

            pool
                .Set(entity0x1, 20)
                .RemoveUnchecked(entity0x1);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity0x1));
        }

        [Test]
        public void ReadOnlySpan()
        {
            var world = (World)World.Create();
            var pool = new Pool<int>(world);

            for (var i = 0; i < 4; i++)
            {
                pool.Get(world.Entities.Create()) = i + 1;
            }

            var span = pool.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual((new ECS.Entity(i, 1), i + 1), span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var world = (World)World.Create();
            var pool = new Pool<int>(world);

            for (var i = 0; i < 4; i++)
            {
                pool.Get(world.Entities.Create()) = i + 1;
            }

            var j = 0;
            foreach (var value in pool)
            {
                Assert.AreEqual((new ECS.Entity(j, 1), j + 1), value);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}