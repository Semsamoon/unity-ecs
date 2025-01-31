using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class EntitiesTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(1, new Entities(10, 10, 10, 10).Length);
            Assert.DoesNotThrow(() => new Entities(-10, -10, -10, -10));

            // Initial length must be 1
            Assert.AreEqual(1, new Entities().Length);
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
            entities.Remove(entities.Create());

            Assert.AreEqual(entities.Length, 1);
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
            var entities = new Entities(1, 1, 1, 1);
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
            Assert.AreEqual(1, entities.Length);
            Assert.DoesNotThrow(() => entities.Contains(new Entity(1, 0)));
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var entities = new Entities();

            // Foreach-loop
            foreach (var i in entities.AsReadOnlySpan())
            {
                Assert.AreEqual(new Entity(0, 0), i);
            }

            for (var i = 1; i < 3; i++)
            {
                entities.Create();
            }

            var span = entities.AsReadOnlySpan();

            // For-loop
            for (var i = 1; i < 3; i++)
            {
                Assert.AreEqual(new Entity(i, 0), span[i]);
            }
        }
    }
}