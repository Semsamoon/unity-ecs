using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class EntityTests
    {
        [Test]
        public void EntityConstructorSetsValidValues()
        {
            var entity = new Entity(10, 10);
            Assert.AreEqual(10, entity.Id);
            Assert.AreEqual(10, entity.Gen);
        }

        [Test]
        public void SameEntitiesAreEqual()
        {
            var entity1 = new Entity(10, 10);
            var entity2 = new Entity(10, 10);
            Assert.True(entity1 == entity2);
            Assert.False(entity1 != entity2);
            Assert.True(entity1.Equals(entity2));
            Assert.True(entity1.Equals((object)entity2));
        }

        [Test]
        public void InvalidEntityIdentifierIsNull()
        {
            // Both ID and Gen are invalid
            Assert.True(new Entity(-1, -1).IsNull());
            Assert.True(new Entity(int.MinValue, int.MinValue).IsNull());

            // ID is 0 (as for NULL-entity) and Gen is invalid
            Assert.True(new Entity(0, -1).IsNull());
            Assert.True(new Entity(0, int.MinValue).IsNull());
        }

        [Test]
        public void DefaultEntityIsNull()
        {
            Assert.True(new Entity().IsNull());
        }

        [Test]
        public void ValidEntityIdentifierIsNotNull()
        {
            // Both ID and Gen are valid
            Assert.False(new Entity(1, 1).IsNull());
            Assert.False(new Entity(int.MaxValue, int.MaxValue).IsNull());

            // ID is valid and Gen is invalid
            Assert.False(new Entity(1, -1).IsNull());
            Assert.False(new Entity(1, int.MinValue).IsNull());
        }

        [Test]
        public void SameEntitiesHashCodesAreEqual()
        {
            var entity1 = new Entity(10, 10);
            var entity2 = new Entity(10, 10);
            Assert.True(entity1.GetHashCode() == entity2.GetHashCode());
        }

        [Test]
        public void ValidEntityStringIsCorrect()
        {
            Assert.AreEqual(new Entity(10, 10).ToString(), "Entity(10; 10)");
            Assert.AreEqual(new Entity(10, int.MaxValue).ToString(), $"Entity(10; {int.MaxValue})");
            Assert.AreEqual(new Entity(int.MaxValue, 10).ToString(), $"Entity({int.MaxValue}; 10)");
        }

        [Test]
        public void NullEntityStringIsCorrect()
        {
            Assert.AreEqual(new Entity().ToString(), "NULL-entity");
        }

        [Test]
        public void DifferentNullEntitiesAreNotEqual()
        {
            Assert.AreNotEqual(new Entity(0, 1), new Entity(0, 2));
            Assert.AreNotEqual(new Entity(0, int.MinValue), new Entity(0, int.MaxValue));
        }
    }
}