using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class EntitiesTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(0, new Entities(10, 10).Length);
            Assert.DoesNotThrow(() => new Entities(-10, -10));

            Assert.AreEqual(0, new Entities().Length);
        }

        [Test]
        public void CanCreateEntity()
        {
            var entities = new Entities();
            Assert.AreEqual(entities.Create(), new Entity(1, 0));
            Assert.AreEqual(entities.Create(), new Entity(2, 0));
        }

        [Test]
        public void CanRecycleEntity()
        {
            var entities = new Entities();
            var entity = entities.Create();
            Assert.AreEqual(entities.Length, 1);

            entities.Remove(entity);
            Assert.AreEqual(entities.Length, 0);

            Assert.AreEqual(entities.Create(), new Entity(1, 1));
        }

        [Test]
        public void ContainsIsCorrect()
        {
            var entities = new Entities();
            entities.Create();

            Assert.True(entities.Contains(new Entity(1, 0)));
            Assert.False(entities.Contains(new Entity(1, 1)));
            Assert.False(entities.Contains(new Entity(2, 0)));
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var entities = new Entities(1, 1);
            Assert.DoesNotThrow(() => entities.Create());
            Assert.DoesNotThrow(() => entities.Create());
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var entities = new Entities();
            Assert.DoesNotThrow(() => entities.Remove(new Entity(1, 0)));

            entities.Create();
            Assert.DoesNotThrow(() => entities.Remove(new Entity(1, 0)));
            Assert.AreEqual(0, entities.Length);
            Assert.DoesNotThrow(() => entities.Contains(new Entity(1, 0)));
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var entities = new Entities();

            for (var i = 0; i < 3; i++)
            {
                entities.Create();
            }

            var j = 0;

            // Foreach-loop
            foreach (var i in entities.AsReadOnlySpan())
            {
                j++;
                Assert.AreEqual(new Entity(j, 0), i);
            }

            var span = entities.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(new Entity(i + 1, 0), span[i]);
            }
        }

        [Test]
        public void EnumerableIsCorrect()
        {
            var entities = new Entities();

            for (var i = 0; i < 3; i++)
            {
                entities.Create();
            }

            var j = 0;

            foreach (var i in entities)
            {
                j++;
                Assert.AreEqual(new Entity(j, 0), i);
            }
        }
    }
}