using System;
using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Filter
    {
        [Test]
        public void Constructor()
        {
            var filter10 = new ECS.Filter(10);
            var filter_10 = new ECS.Filter(-10);
            var filter0x10x10 = new ECS.Filter(0, 10, 10);
            var filter0x_10x_10 = new ECS.Filter(0, -10, -10);

            Assert.Positive(filter10.Capacity);
            Assert.Positive(filter_10.Capacity);
            Assert.AreEqual(10, filter0x10x10.Capacity);
            Assert.Positive(filter0x_10x_10.Capacity);

            Assert.AreEqual(0, filter10.Length);
            Assert.AreEqual(0, filter_10.Length);
            Assert.AreEqual(0, filter0x10x10.Length);
            Assert.AreEqual(0, filter0x_10x_10.Length);
        }

        [Test]
        public void Getter()
        {
            var filter0 = new ECS.Filter(0);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);
            var included = Array.Empty<IContains>();
            var excluded = Array.Empty<IContains>();

            filter0.Recheck(entity1x0, included, excluded);
            filter0.Recheck(entity2x0, included, excluded);

            Assert.AreEqual(entity1x0, filter0[0]);
            Assert.AreEqual(entity2x0, filter0[1]);
            Assert.AreEqual(entity, filter0[2]);
        }

        [Test]
        public void Recheck()
        {
            var filter0 = new ECS.Filter(0);
            var filter1 = new ECS.Filter(1);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);
            var included = Array.Empty<IContains>();
            var excluded = Array.Empty<IContains>();

            filter0.Recheck(entity1x0, included, excluded);
            filter1.Recheck(entity1x0, included, excluded);

            Assert.AreEqual(1, filter0.Length);
            Assert.AreEqual(entity1x0, filter0[0]);
            Assert.AreEqual(0, filter1.Length);

            filter0.Recheck(entity, included, excluded);
            filter0.Recheck(entity1x0, included, excluded);
            filter0.Recheck(entity2x0, included, excluded);
            filter1.Recheck(entity, included, excluded);
            filter1.Recheck(entity2x0, included, excluded);

            Assert.AreEqual(2, filter0.Length);
            Assert.AreEqual(entity2x0, filter0[1]);
            Assert.AreEqual(0, filter1.Length);
        }

        [Test]
        public void Change()
        {
            var filter1 = new ECS.Filter(1);
            var entity1x0 = new ECS.Entity(1, 0);
            var entity2x0 = new ECS.Entity(2, 0);

            filter1.Change(entity1x0, 1);

            Assert.AreEqual(1, filter1.Length);
            Assert.AreEqual(entity1x0, filter1[0]);

            filter1.Change(entity1x0, -1);
            filter1.Change(entity2x0, 1);

            Assert.AreEqual(1, filter1.Length);
            Assert.AreEqual(entity2x0, filter1[0]);
        }

        [Test]
        public void Contains()
        {
            var filter0 = new ECS.Filter(0);
            var entity = new ECS.Entity();
            var entity1x0 = new ECS.Entity(1, 0);
            var entity1x1 = new ECS.Entity(1, 1);
            var included = Array.Empty<IContains>();
            var excluded = Array.Empty<IContains>();

            filter0.Recheck(entity1x0, included, excluded);

            Assert.True(filter0.Contains(entity1x0));
            Assert.False(filter0.Contains(entity));
            Assert.False(filter0.Contains(entity1x1));
        }

        [Test]
        public void ReadOnlySpan()
        {
            var filter0 = new ECS.Filter(0);
            var included = Array.Empty<IContains>();
            var excluded = Array.Empty<IContains>();

            for (var i = 0; i < 4; i++)
            {
                filter0.Recheck(new ECS.Entity(i + 1, 0), included, excluded);
            }

            var span = filter0.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(new ECS.Entity(i + 1, 0), span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var filter0 = new ECS.Filter(0);
            var included = Array.Empty<IContains>();
            var excluded = Array.Empty<IContains>();

            for (var i = 0; i < 4; i++)
            {
                filter0.Recheck(new ECS.Entity(i + 1, 0), included, excluded);
            }

            var j = 0;
            foreach (var entity in filter0)
            {
                Assert.AreEqual(new ECS.Entity(j + 1, 0), entity);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}