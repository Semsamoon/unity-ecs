using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Filters
    {
        [Test]
        public void Constructor()
        {
            var filters = new ECS.Filters(null);
            var filters10x10 = new ECS.Filters(null, 10, 10);
            var filters_10x_10 = new ECS.Filters(null, -10, -10);

            Assert.AreEqual((0, 0), filters.Length);
            Assert.AreEqual((0, 0), filters10x10.Length);
            Assert.AreEqual((0, 0), filters_10x_10.Length);
        }

        [Test]
        public void Build()
        {
            var world = new World();
            var entity1x0 = world.Entities.Create();
            var entity2x0 = world.Entities.Create();
            var filterInt = world.Filters.Create().Include<int>().Build();

            world.Pools.Get<int>().Set(entity1x0, 10);
            world.Pools.Get<string>().Set(entity1x0, "10");

            Assert.AreEqual((1, 0), world.FiltersInternal.Length);
            Assert.True(filterInt.Contains(entity1x0));
            Assert.False(filterInt.Contains(entity2x0));

            var filterIntString = world.Filters.Create(2, 2).Include<int, string>().Build();
            var filter_String = world.Filters.Create(4, 4).Exclude<string>().Build();

            Assert.AreEqual((2, 1), world.FiltersInternal.Length);
            Assert.True(filterIntString.Contains(entity1x0));
            Assert.False(filterIntString.Contains(entity2x0));
            Assert.False(filter_String.Contains(entity1x0));
            Assert.True(filter_String.Contains(entity2x0));
        }

        [Test]
        public void Include()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(2);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Include(filter, typeof(int));
            filters.Include(filter, typeof(string), 4);

            filters.Record(entity1x0, typeof(int));
            filters.Record(entity1x0, typeof(string));
            filters.Record(entity2x0, typeof(int));

            Assert.AreEqual((2, 0), filters.Length);
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Exclude()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(2);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Exclude(filter, typeof(int));
            filters.Exclude(filter, typeof(string), 4);

            filters.Erase(entity1x0, typeof(int));
            filters.Erase(entity1x0, typeof(string));
            filters.Record(entity2x0, typeof(int));
            filters.Erase(entity2x0, typeof(string));

            Assert.AreEqual((0, 2), filters.Length);
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Create()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(1);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Include(filter, typeof(int));

            filters.Record(entity, typeof(int));
            filters.Record(entity1x0, typeof(int));
            filters.Record(entity1x0, typeof(string));
            filters.Record(entity2x0, typeof(string));

            Assert.False(filter.Contains(entity));
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }

        [Test]
        public void Remove()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(1);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filters.Exclude(filter, typeof(int));

            filters.Erase(entity, typeof(int));
            filters.Erase(entity1x0, typeof(int));
            filters.Erase(entity1x0, typeof(string));
            filters.Erase(entity2x0, typeof(string));

            Assert.False(filter.Contains(entity));
            Assert.True(filter.Contains(entity1x0));
            Assert.False(filter.Contains(entity2x0));
        }
    }
}