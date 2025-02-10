using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Pool
    {
        [Test]
        public void Constructor()
        {
            var pool = new ECS.Pool(null, typeof(int));
            var pool10x10 = new ECS.Pool(null, typeof(int), new OptionsPool(10, 10));
            var pool_10x_10 = new ECS.Pool(null, typeof(int), new OptionsPool(-10, -10));

            Assert.AreEqual(10, pool10x10.Capacity);
            Assert.Positive(pool.Capacity);
            Assert.Positive(pool_10x_10.Capacity);

            Assert.AreEqual(0, pool.Length);
            Assert.AreEqual(0, pool10x10.Length);
            Assert.AreEqual(0, pool_10x_10.Length);
        }

        [Test]
        public void Getter()
        {
            var world = new World();
            var pool = new ECS.Pool(world, typeof(int));
            var entity = new ECS.Entity();
            var entity1x0 = world.Entities.Create();
            var entity2x0 = world.Entities.Create();

            pool.Add(entity1x0);
            pool.Add(entity2x0);

            Assert.AreEqual(entity1x0, pool[0]);
            Assert.AreEqual(entity2x0, pool[1]);
            Assert.AreEqual(entity, pool[2]);
        }

        [Test]
        public void Add()
        {
            var world = new World();
            var pool = new ECS.Pool(world, typeof(int));
            var entity = new ECS.Entity();
            var entity1x0 = world.Entities.Create();
            var entity2x0 = world.Entities.Create();

            pool.Add(entity1x0);

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(entity1x0, pool[0]);

            pool.Add(entity1x0);
            pool.Add(entity2x0);
            pool.Add(entity);

            Assert.AreEqual(2, pool.Length);
            Assert.AreEqual(entity2x0, pool[1]);
        }

        [Test]
        public void Contains()
        {
            var world = new World();
            var pool = new ECS.Pool(world, typeof(int));
            var entity = new ECS.Entity();
            var entity1x0 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool.Add(entity1x0);

            Assert.True(pool.Contains(entity1x0));
            Assert.False(pool.Contains(entity1x1));
            Assert.False(pool.Contains(entity));
        }

        [Test]
        public void Extending()
        {
            var world = new World();
            var pool = new ECS.Pool(world, typeof(int), new OptionsPool(2, 2));

            for (var i = 0; i < 33; i++)
            {
                pool.Add(world.Entities.Create());
            }

            Assert.AreEqual(64, pool.Capacity);
            Assert.AreEqual(new ECS.Entity(32, 1), pool[32]);
            Assert.AreEqual(new ECS.Entity(), pool[33]);
        }

        [Test]
        public void Remove()
        {
            var world = new World();
            var pool = new ECS.Pool(world, typeof(int));
            var entity1x0 = world.Entities.Create();

            Assert.DoesNotThrow(() => pool.Remove(entity1x0));

            pool.Add(entity1x0);
            pool.Remove(entity1x0);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity1x0));
        }

        [Test]
        public void ReadOnlySpan()
        {
            var world = new World();
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
            var world = new World();
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
    }

    public sealed class Pool_T
    {
        [Test]
        public void Constructor()
        {
            var world = new World();
            var pool = new Pool<int>(world);
            var pool10x10 = new Pool<int>(world, new OptionsPool(10, 10));
            var pool_10x_10 = new Pool<int>(world, new OptionsPool(-10, -10));

            Assert.AreEqual(10, pool10x10.Capacity);
            Assert.Positive(pool.Capacity);
            Assert.Positive(pool_10x_10.Capacity);

            Assert.AreEqual(0, pool.Length);
            Assert.AreEqual(0, pool10x10.Length);
            Assert.AreEqual(0, pool_10x_10.Length);
        }

        [Test]
        public void Getter()
        {
            var world = new World();
            var pool = new Pool<int>(world);
            var entity = new ECS.Entity();
            var entity1x0 = world.Entities.Create();
            var entity2x0 = world.Entities.Create();
            var entity3x0 = world.Entities.Create();

            pool.Get(entity1x0) = 10;
            pool.Get(entity2x0) = 20;

            Assert.AreEqual((entity1x0, 10), pool[0]);
            Assert.AreEqual((entity2x0, 20), pool[1]);
            Assert.AreEqual((entity, 0), pool[2]);
            Assert.AreEqual(10, pool.GetUnchecked(0));
            Assert.AreEqual(20, pool.Get(entity2x0));
            Assert.AreEqual(0, pool.Get(entity3x0));
        }

        [Test]
        public void Set()
        {
            var world = new World();
            var pool = new Pool<int>(world);
            var entity1x0 = world.Entities.Create();
            var entity2x0 = world.Entities.Create();

            pool.Get(entity1x0) = 10;

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual((entity1x0, 10), pool[0]);

            pool.Get(entity1x0) = 20;
            pool.Get(entity2x0) = 30;

            Assert.AreEqual(2, pool.Length);
            Assert.AreEqual((entity1x0, 20), pool[0]);
            Assert.AreEqual((entity2x0, 30), pool[1]);
        }

        [Test]
        public void Contains()
        {
            var world = new World();
            var pool = new Pool<int>(world);
            var entity = new ECS.Entity();
            var entity1x0 = world.Entities.Create();
            var entity1x1 = world.Entities.Create();

            pool.Get(entity1x0) = 10;

            Assert.True(pool.Contains(entity1x0));
            Assert.False(pool.Contains(entity1x1));
            Assert.False(pool.Contains(entity));
        }

        [Test]
        public void Extending()
        {
            var world = new World();
            var pool = new Pool<int>(world, new OptionsPool(2, 2));

            for (var i = 0; i < 33; i++)
            {
                pool.Get(world.Entities.Create()) = i + 1;
            }

            Assert.AreEqual(64, pool.Capacity);
            Assert.AreEqual((new ECS.Entity(32, 1), 33), pool[32]);
            Assert.AreEqual((new ECS.Entity(), 0), pool[33]);
        }

        [Test]
        public void Remove()
        {
            var world = new World();
            var pool = new Pool<int>(world);
            var entity1x0 = world.Entities.Create();

            Assert.DoesNotThrow(() => pool.Remove(entity1x0));

            pool.Get(entity1x0) = 10;
            pool.Remove(entity1x0);

            Assert.AreEqual(0, pool.Length);
            Assert.False(pool.Contains(entity1x0));
        }

        [Test]
        public void ReadOnlySpan()
        {
            var world = new World();
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
            var world = new World();
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