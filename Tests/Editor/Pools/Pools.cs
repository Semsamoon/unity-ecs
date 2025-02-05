using System;
using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Pools
    {
        [Test]
        public void Constructor()
        {
            var pools = new ECS.Pools();
            var pools10 = new ECS.Pools(10);
            var pools_10 = new ECS.Pools(-10);

            Assert.AreEqual(0, pools.Length);
            Assert.AreEqual(0, pools10.Length);
            Assert.AreEqual(0, pools_10.Length);
        }

        [Test]
        public void Add()
        {
            var pools = new ECS.Pools();

            pools.Add<int>();
            pools.Add<int>(10, 10);
            pools.Add<string>(10, 10);

            Assert.AreEqual(2, pools.Length);
            Assert.AreNotEqual(10, pools.Get<int>().Capacity);
            Assert.AreEqual(10, pools.Get<string>().Capacity);

            pools.Add<ATag>();
            pools.Add<ATag>(10, 10);
            pools.Add<BTag>(10, 10);

            Assert.AreEqual(4, pools.Length);
            Assert.AreNotEqual(10, pools.GetTag<ATag>().Capacity);
            Assert.AreEqual(10, pools.GetTag<BTag>().Capacity);
        }

        [Test]
        public void Get()
        {
            var pools = new ECS.Pools();

            pools.Add<int>(10, 10);

            Assert.AreEqual(10, pools.Get<int>(20, 20).Capacity);
            Assert.AreEqual(10, pools.Get<string>(10, 10).Capacity);
            Assert.AreEqual(10, pools.Get<string>().Capacity);

            pools.Add<ATag>(10, 10);

            Assert.AreEqual(10, pools.GetTag<ATag>(20, 20).Capacity);
            Assert.AreEqual(10, pools.GetTag<BTag>(10, 10).Capacity);
            Assert.AreEqual(10, pools.GetTag<BTag>().Capacity);
            Assert.Throws<InvalidCastException>(() => pools.Get<ATag>());
        }

        [Test]
        public void Contains()
        {
            var pools = new ECS.Pools();

            pools.Add<int>();
            pools.Add<ATag>();

            Assert.True(pools.Contains<int>());
            Assert.True(pools.Contains<ATag>());
            Assert.False(pools.Contains<string>());
            Assert.False(pools.Contains<BTag>());
        }

        [Test]
        public void Remove()
        {
            var pools = new ECS.Pools();

            Assert.DoesNotThrow(() => pools.Remove<int>());

            pools.Add<int>();
            pools.Add<ATag>();
            pools.Remove<int>();
            pools.Remove<ATag>();

            Assert.False(pools.Contains<int>());
            Assert.False(pools.Contains<ATag>());
        }

        [Test]
        public void Enumerable()
        {
            var pools = new ECS.Pools();

            pools.Add<int>();
            pools.Add<ATag>();

            var j = 0;
            foreach (var pool in pools)
            {
                Assert.IsTrue(pool is Pool<int> or ECS.Pool);
                j++;
            }

            Assert.AreEqual(2, j);
        }

        private struct ATag : ITag
        {
        }

        private struct BTag : ITag
        {
        }
    }
}