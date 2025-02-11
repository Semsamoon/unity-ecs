using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Entities
    {
        [Test]
        public void Constructor()
        {
            var entities = new ECS.Entities(null);
            var entities10x10 = new ECS.Entities(null, new OptionsEntities(10, 10));
            var entities_10x_10 = new ECS.Entities(null, new OptionsEntities(-10, -10));

            Assert.AreEqual(10, entities10x10.Capacity);
            Assert.Positive(entities.Capacity);
            Assert.Positive(entities_10x_10.Capacity);

            Assert.AreEqual(0, entities.Length);
            Assert.AreEqual(0, entities10x10.Length);
            Assert.AreEqual(0, entities_10x_10.Length);
        }

        [Test]
        public void Getter()
        {
            var entities = new ECS.Entities(null);
            var entity = new ECS.Entity();
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            entities.Create();
            entities.Create();

            Assert.AreEqual(entity0x1, entities[0].Entity);
            Assert.AreEqual(entity1x1, entities[1].Entity);
            Assert.AreEqual(entity, entities[2].Entity);
            Assert.AreEqual(0, entities[0].Components.Length);
            Assert.AreEqual(0, entities[1].Components.Length);
            Assert.IsNull(entities[2].Components);
        }

        [Test]
        public void Create()
        {
            var world = new World();
            var entities = new ECS.Entities(world);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity0x2 = new ECS.Entity(0, 2);
            var entity1x1 = new ECS.Entity(1, 1);
            var entity1x2 = new ECS.Entity(1, 2);

            entities.Create();

            Assert.AreEqual(1, entities.Length);
            Assert.AreEqual(entity0x1, entities[0].Entity);
            Assert.AreEqual(0, entities[0].Components.Length);

            entities
                .Remove(entity0x1)
                .Create(10);

            Assert.AreEqual(1, entities.Length);
            Assert.AreEqual(entity0x2, entities[0].Entity);
            Assert.GreaterOrEqual(entities[0].Components.Capacity, 10);
            Assert.AreEqual(0, entities[0].Components.Length);

            entities.CreateUnchecked(10);

            Assert.AreEqual(2, entities.Length);
            Assert.AreEqual(entity1x1, entities[1].Entity);
            Assert.AreEqual(10, entities[1].Components.Capacity);
            Assert.AreEqual(0, entities[1].Components.Length);

            entities
                .Remove(entity1x1)
                .RecycleUnchecked();

            Assert.AreEqual(2, entities.Length);
            Assert.AreEqual(entity1x2, entities[1].Entity);
            Assert.AreEqual(10, entities[1].Components.Capacity);
            Assert.AreEqual(0, entities[1].Components.Length);
        }

        [Test]
        public void Contains()
        {
            var entities = new ECS.Entities(null);
            var entity = new ECS.Entity();
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            entities.Create();

            Assert.True(entities.Contains(entity0x1));
            Assert.False(entities.Contains(entity1x1));
            Assert.False(entities.Contains(entity));
        }

        [Test]
        public void Extending()
        {
            var entities = new ECS.Entities(null, new OptionsEntities(2, 2));

            for (var i = 0; i < 33; i++)
            {
                entities.Create();
            }

            Assert.AreEqual(64, entities.Capacity);
            Assert.AreEqual(new ECS.Entity(32, 1), entities[32].Entity);
            Assert.AreEqual(new ECS.Entity(), entities[33].Entity);
        }

        [Test]
        public void Remove()
        {
            var world = new World();
            var entities = world.EntitiesInternal;
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            Assert.DoesNotThrow(() => entities.Remove(entity0x1));

            entities.Create();
            world.PoolsInternal.Get<int>().Get(entity0x1) = 10;
            entities.Remove(entity0x1);

            Assert.AreEqual(0, entities.Length);
            Assert.AreEqual(0, entities[1].Components.Length);
            Assert.False(entities.Contains(entity0x1));

            entities.Create();
            entities.RemoveUnchecked(entity1x1);

            Assert.AreEqual(0, entities.Length);
            Assert.False(entities.Contains(entity1x1));
        }

        [Test]
        public void Record()
        {
            var world = new World();
            var entities = world.EntitiesInternal;
            var entity0x1 = entities.Create();

            entities.RecordUnchecked(entity0x1, typeof(int));

            Assert.AreEqual(1, entities[0].Components.Length);
            Assert.AreEqual(typeof(int), entities[0].Components[0]);
        }

        [Test]
        public void Erase()
        {
            var world = new World();
            var entities = world.EntitiesInternal;
            var entity0x1 = entities.Create();

            entities
                .RecordUnchecked(entity0x1, typeof(int))
                .EraseUnchecked(entity0x1, typeof(int));

            Assert.AreEqual(0, entities[0].Components.Length);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var entities = new ECS.Entities(null);

            for (var i = 0; i < 4; i++)
            {
                entities.Create();
            }

            var span = entities.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(new ECS.Entity(i, 1), span[i].Entity);
            }
        }

        [Test]
        public void Enumerable()
        {
            var entities = new ECS.Entities(null);

            for (var i = 0; i < 4; i++)
            {
                entities.Create();
            }

            var j = 0;
            foreach (var (entity, _) in entities)
            {
                Assert.AreEqual(new ECS.Entity(j, 1), entity);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}