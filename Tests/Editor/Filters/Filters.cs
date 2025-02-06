using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Filters
    {
        [Test]
        public void Constructor()
        {
            var filters = new ECS.Filters();
            var filters10x10 = new ECS.Filters(10, 10);
            var filters_10x_10 = new ECS.Filters(-10, -10);

            Assert.AreEqual((0, 0), filters.Length);
            Assert.AreEqual((0, 0), filters10x10.Length);
            Assert.AreEqual((0, 0), filters_10x_10.Length);
        }

        [Test]
        public void Include()
        {
            var filters = new ECS.Filters();
            var filter = new ECS.Filter(2);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Include(filter, typeof(int));
            filters.Include(filter, typeof(string), 4);

            filters.Create(entity1x0, typeof(int));
            filters.Create(entity1x0, typeof(string));
            filters.Create(entity2x0, typeof(int));

            Assert.AreEqual((2, 0), filters.Length);
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Exclude()
        {
            var filters = new ECS.Filters();
            var filter = new ECS.Filter(2);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Exclude(filter, typeof(int));
            filters.Exclude(filter, typeof(string), 4);

            filters.Remove(entity1x0, typeof(int));
            filters.Remove(entity1x0, typeof(string));
            filters.Create(entity2x0, typeof(int));
            filters.Remove(entity2x0, typeof(string));

            Assert.AreEqual((0, 2), filters.Length);
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Create()
        {
            var filters = new ECS.Filters();
            var filter = new ECS.Filter(1);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Include(filter, typeof(int));

            filters.Create(entity, typeof(int));
            filters.Create(entity1x0, typeof(int));
            filters.Create(entity1x0, typeof(string));
            filters.Create(entity2x0, typeof(string));

            Assert.False(filter.Contains(entity));
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Remove()
        {
            var filters = new ECS.Filters();
            var filter = new ECS.Filter(1);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Exclude(filter, typeof(int));

            filters.Remove(entity, typeof(int));
            filters.Remove(entity1x0, typeof(int));
            filters.Remove(entity1x0, typeof(string));
            filters.Remove(entity2x0, typeof(string));

            Assert.False(filter.Contains(entity));
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }
    }
}