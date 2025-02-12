using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Entity
    {
        [Test]
        public void Constructor()
        {
            var entity10x10 = new ECS.Entity(10, 10);
            Assert.AreEqual(10, entity10x10.Id);
            Assert.AreEqual(10, entity10x10.Gen);

            var entity = new ECS.Entity();
            Assert.AreEqual(0, entity.Id);
            Assert.AreEqual(0, entity.Gen);
        }

        [Test]
        public void Operators()
        {
            var entity10x10_1 = new ECS.Entity(10, 10);
            var entity10x10_2 = new ECS.Entity(10, 10);

            Assert.True(entity10x10_1 == entity10x10_2);
            Assert.False(entity10x10_1 != entity10x10_2);
            Assert.True(entity10x10_1.Equals(entity10x10_2));
            Assert.True(entity10x10_1.Equals((object)entity10x10_2));

            var entity0x0 = new ECS.Entity(0, 0);
            var entity0x1 = new ECS.Entity(0, 1);
            Assert.True(entity0x0 != entity0x1);
        }

        [Test]
        public void Null()
        {
            var entity = new ECS.Entity();
            var entity0x1 = new ECS.Entity(0, 1);
            var entity10x10 = new ECS.Entity(10, 10);

            Assert.True(entity == ECS.Entity.Null);
            Assert.True(entity0x1 != ECS.Entity.Null);
            Assert.True(entity10x10 != ECS.Entity.Null);
        }

        [Test]
        public void HashCode()
        {
            var entity10x10 = new ECS.Entity(10, 10);
            var entity20x20 = new ECS.Entity(20, 20);

            Assert.AreEqual(entity10x10.GetHashCode(), entity10x10.GetHashCode());
            Assert.AreNotEqual(entity10x10.GetHashCode(), entity20x20.GetHashCode());
        }

        [Test]
        public void String()
        {
            var entity = new ECS.Entity();
            var entity10x10 = new ECS.Entity(10, 10);

            Assert.AreEqual(entity.ToString(), "[NULL]");
            Assert.AreEqual(entity10x10.ToString(), "[10; 10]");
        }
    }
}