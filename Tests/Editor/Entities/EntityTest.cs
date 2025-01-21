using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class EntityTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(10, new Entity(10, 10).Id);
            Assert.AreEqual(10, new Entity(10, 10).Gen);

            Assert.AreEqual(0, new Entity(0, -1).Id);
            Assert.AreEqual(0, new Entity(-1, 0).Gen);

            Assert.AreEqual(0, new Entity(-1, -1).Id);
            Assert.AreEqual(0, new Entity(-1, -1).Gen);

            Assert.AreEqual(0, new Entity().Id);
            Assert.AreEqual(0, new Entity().Gen);
        }

        [Test]
        public void EqualityOperatorsAreCorrect()
        {
            var entity1 = new Entity(10, 10);
            var entity2 = new Entity(10, 10);

            Assert.True(entity1 == entity2);
            Assert.False(entity1 != entity2);
            Assert.True(entity1.Equals(entity2));
            Assert.True(entity1.Equals((object)entity2));

            // NULL-entities are not equal if their generation numbers are different
            Assert.False(new Entity(0, 1) == new Entity(0, 2));
        }

        [Test]
        public void NullCheckIsCorrect()
        {
            Assert.False(new Entity(int.MaxValue, int.MaxValue).IsNull());
            Assert.False(new Entity(1, int.MinValue).IsNull());
            Assert.True(new Entity().IsNull());
            Assert.True(new Entity(0, int.MaxValue).IsNull());
            Assert.True(new Entity(int.MinValue, int.MinValue).IsNull());
        }

        [Test]
        public void HashCodesAreDifferent()
        {
            Assert.AreNotEqual(new Entity(10, 20).GetHashCode(), new Entity(20, 10).GetHashCode());
            Assert.AreEqual(new Entity(10, 10).GetHashCode(), new Entity(10, 10).GetHashCode());
        }

        [Test]
        public void ToStringIsCorrect()
        {
            Assert.AreEqual(new Entity(10, 10).ToString(), "Entity(10; 10)");
            Assert.AreEqual(new Entity().ToString(), "NULL-entity");
        }
    }
}