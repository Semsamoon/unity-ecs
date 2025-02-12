using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Filters
    {
        [Test]
        public void Constructor()
        {
            var filters = new ECS.Filters(null);
            var filters10x10 = new ECS.Filters(null, new OptionsFilters(10, 10), OptionsFilter.Default, OptionsEntities.Default);

            Assert.AreEqual((0, 0), filters.Length);
            Assert.AreEqual((0, 0), filters10x10.Length);
        }

        [Test]
        public void Create()
        {
            var world = (World)World.Create();
            var filters = world.FiltersInternal;
            var entity0x1 = world.EntitiesInternal.Create();
            var entity1x1 = world.EntitiesInternal.Create();
            var filterInt = filters.Create().Include<int>().Build();

            world.PoolsInternal.Get<int>().Get(entity0x1) = 10;
            world.PoolsInternal.Get<string>().Get(entity0x1) = "10";

            Assert.AreEqual((1, 0), filters.Length);
            Assert.True(filterInt.Contains(entity0x1));
            Assert.False(filterInt.Contains(entity1x1));

            var filterIntString = filters.Create().Include<int, string>().Build();
            var filter_String = filters.Create(new OptionsFilter(10)).Exclude<string>().Build();

            Assert.AreEqual((2, 1), filters.Length);
            Assert.True(filterIntString.Contains(entity0x1));
            Assert.False(filterIntString.Contains(entity1x1));
            Assert.False(filter_String.Contains(entity0x1));
            Assert.True(filter_String.Contains(entity1x1));
        }

        [Test]
        public void Capacity()
        {
            var world = (World)World.Create();
            var filters = world.FiltersInternal;

            Assert.DoesNotThrow(() => filters.IncludeCapacity<int>(10).ExcludeCapacity<int>(10));
        }

        [Test]
        public void Include()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(2);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filters
                .Include(filter, typeof(int))
                .Include(filter, typeof(string), 4)
                .RecordUnchecked(entity0x1, typeof(int))
                .RecordUnchecked(entity0x1, typeof(string))
                .RecordUnchecked(entity1x1, typeof(int));

            Assert.AreEqual((2, 0), filters.Length);
            Assert.True(filter.Contains(entity0x1));
            Assert.False(filter.Contains(entity1x1));
        }

        [Test]
        public void Exclude()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(2);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filters
                .Exclude(filter, typeof(int))
                .Exclude(filter, typeof(string), 4)
                .EraseUnchecked(entity0x1, typeof(int))
                .EraseUnchecked(entity0x1, typeof(string))
                .RecordUnchecked(entity1x1, typeof(int))
                .EraseUnchecked(entity1x1, typeof(string));

            Assert.AreEqual((0, 2), filters.Length);
            Assert.True(filter.Contains(entity0x1));
            Assert.False(filter.Contains(entity1x1));
        }

        [Test]
        public void Record()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(1);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filters
                .Include(filter, typeof(int))
                .RecordUnchecked(entity0x1, typeof(int))
                .RecordUnchecked(entity0x1, typeof(string))
                .RecordUnchecked(entity1x1, typeof(string));

            Assert.True(filter.Contains(entity0x1));
            Assert.False(filter.Contains(entity1x1));
        }

        [Test]
        public void Erase()
        {
            var filters = new ECS.Filters(null);
            var filter = new ECS.Filter(1);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filters
                .Exclude(filter, typeof(int))
                .EraseUnchecked(entity0x1, typeof(int))
                .EraseUnchecked(entity0x1, typeof(string))
                .EraseUnchecked(entity1x1, typeof(string));

            Assert.True(filter.Contains(entity0x1));
            Assert.False(filter.Contains(entity1x1));
        }
    }
}