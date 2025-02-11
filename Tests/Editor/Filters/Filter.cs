using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Filter
    {
        [Test]
        public void Constructor()
        {
            var filter = new ECS.Filter();
            var filter0x10x10 = new ECS.Filter(0, new OptionsFilter(10, 10));
            var filter1x_10x_10 = new ECS.Filter(1, new OptionsFilter(-10, -10));

            Assert.Positive(filter.Capacity);
            Assert.AreEqual(10, filter0x10x10.Capacity);
            Assert.Positive(filter1x_10x_10.Capacity);

            Assert.AreEqual(0, filter.Length);
            Assert.AreEqual(0, filter0x10x10.Length);
            Assert.AreEqual(0, filter1x_10x_10.Length);

            Assert.AreEqual(0, filter.Sum);
            Assert.AreEqual(0, filter0x10x10.Sum);
            Assert.AreEqual(1, filter1x_10x_10.Sum);
        }

        [Test]
        public void Getter()
        {
            var filter = new ECS.Filter();
            var entity = new ECS.Entity();
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filter
                .ChangeUnchecked(entity0x1, 0)
                .ChangeUnchecked(entity1x1, 0);

            Assert.AreEqual(entity0x1, filter[0]);
            Assert.AreEqual(entity1x1, filter[1]);
            Assert.AreEqual(entity, filter[2]);
        }

        [Test]
        public void Change()
        {
            var filter1 = new ECS.Filter(1);
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filter1.ChangeUnchecked(entity0x1, 1);

            Assert.AreEqual(1, filter1.Length);
            Assert.AreEqual(entity0x1, filter1[0]);

            filter1
                .ChangeUnchecked(entity0x1, -1)
                .ChangeUnchecked(entity1x1, 1);

            Assert.AreEqual(1, filter1.Length);
            Assert.AreEqual(entity1x1, filter1[0]);
        }

        [Test]
        public void Add()
        {
            var filter = new ECS.Filter();
            var entity0x1 = new ECS.Entity(0, 1);

            filter.AddUnchecked(entity0x1);

            Assert.AreEqual(1, filter.Length);
            Assert.AreEqual(entity0x1, filter[0]);
        }

        [Test]
        public void Contains()
        {
            var filter = new ECS.Filter();
            var entity = new ECS.Entity();
            var entity0x1 = new ECS.Entity(0, 1);
            var entity1x1 = new ECS.Entity(1, 1);

            filter.ChangeUnchecked(entity0x1, 0);

            Assert.True(filter.Contains(entity0x1));
            Assert.False(filter.Contains(entity));
            Assert.False(filter.Contains(entity1x1));
        }

        [Test]
        public void Remove()
        {
            var filter = new ECS.Filter();
            var entity0x1 = new ECS.Entity(0, 1);

            filter
                .AddUnchecked(entity0x1)
                .RemoveUnchecked(entity0x1);

            Assert.AreEqual(0, filter.Length);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var filter = new ECS.Filter();

            for (var i = 0; i < 4; i++)
            {
                filter.ChangeUnchecked(new ECS.Entity(i + 1, 0), 0);
            }

            var span = filter.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(new ECS.Entity(i + 1, 0), span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var filter = new ECS.Filter();

            for (var i = 0; i < 4; i++)
            {
                filter.ChangeUnchecked(new ECS.Entity(i + 1, 0), 0);
            }

            var j = 0;
            foreach (var entity in filter)
            {
                Assert.AreEqual(new ECS.Entity(j + 1, 0), entity);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}