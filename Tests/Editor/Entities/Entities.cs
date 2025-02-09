using System;
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
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            entities.Create();
            entities.Create();

            Assert.AreEqual(entity1x0, entities[0].Entity);
            Assert.AreEqual(entity2x0, entities[1].Entity);
            Assert.AreEqual(entity, entities[2].Entity);
            Assert.IsInstanceOf<DenseArray<Type>>(entities[0].Components);
            Assert.IsInstanceOf<DenseArray<Type>>(entities[1].Components);
            Assert.IsNull(entities[2].Components);
        }

        [Test]
        public void Create()
        {
            var world = new World();
            var entities = new ECS.Entities(world);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity1x1 = new ECS.Entity(1, 1);
            var entity2x0 = new ECS.Entity(2, 0);

            entities.Create();

            Assert.AreEqual(1, entities.Length);
            Assert.AreEqual(entity1x0, entities[0].Entity);
            Assert.AreEqual(0, entities[0].Components.Length);

            entities.Remove(entity1x0);
            entities.Create();

            Assert.AreEqual(1, entities.Length);
            Assert.AreEqual(entity1x1, entities[0].Entity);
            Assert.AreEqual(0, entities[0].Components.Length);

            entities.Create();

            Assert.AreEqual(2, entities.Length);
            Assert.AreEqual(entity2x0, entities[1].Entity);
            Assert.AreEqual(0, entities[1].Components.Length);
        }

        [Test]
        public void Contains()
        {
            var entities = new ECS.Entities(null);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity1x1 = new ECS.Entity(1, 1);

            entities.Create();

            Assert.True(entities.Contains(entity1x0));
            Assert.False(entities.Contains(entity));
            Assert.False(entities.Contains(entity1x1));
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
            Assert.AreEqual(new ECS.Entity(33, 0), entities[32].Entity);
            Assert.AreEqual(new ECS.Entity(), entities[33].Entity);
        }

        [Test]
        public void Remove()
        {
            var world = new World();
            var entities = new ECS.Entities(world);
            var entity1x0 = world.Entities.Create();

            Assert.DoesNotThrow(() => entities.Remove(entity1x0));

            entities.Create();
            world.PoolsInternal.Get<int>().Set(entity1x0, 10);
            entities.Remove(entity1x0);

            Assert.AreEqual(0, entities.Length);
            Assert.AreEqual(0, entities[1].Components.Length);
            Assert.False(entities.Contains(entity1x0));
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
                Assert.AreEqual(new ECS.Entity(i + 1, 0), span[i].Entity);
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
                Assert.AreEqual(new ECS.Entity(j + 1, 0), entity);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}