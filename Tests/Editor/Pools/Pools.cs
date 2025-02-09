﻿using System;
using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Pools
    {
        [Test]
        public void Constructor()
        {
            var pools = new ECS.Pools(null);
            var pools10 = new ECS.Pools(null, new OptionsPools(10), OptionsPool.Default());
            var pools_10 = new ECS.Pools(null, new OptionsPools(-10), OptionsPool.Default());

            Assert.AreEqual(0, pools.Length);
            Assert.AreEqual(0, pools10.Length);
            Assert.AreEqual(0, pools_10.Length);
        }

        [Test]
        public void Add()
        {
            var pools = new ECS.Pools(null);

            pools.Add<int>();
            pools.Add<int>(new OptionsPool(10, 10));
            pools.Add<string>(new OptionsPool(10, 10));

            Assert.AreEqual(2, pools.Length);
            Assert.AreNotEqual(10, ((Pool<int>)pools.Get<int>()).Capacity);
            Assert.AreEqual(10, pools.GetPool<string>().Capacity);

            pools.Add<ATag>();
            pools.Add<ATag>(new OptionsPool(10, 10));
            pools.Add<BTag>(new OptionsPool(10, 10));

            Assert.AreEqual(4, pools.Length);
            Assert.AreNotEqual(10, ((ECS.Pool)pools.GetTag<ATag>()).Capacity);
            Assert.AreEqual(10, pools.GetPool<BTag>().Capacity);
        }

        [Test]
        public void Get()
        {
            var pools = new ECS.Pools(null);

            pools.Add<int>(new OptionsPool(10, 10));

            Assert.AreEqual(10, pools.GetPool<int>(new OptionsPool(20, 20)).Capacity);
            Assert.AreEqual(10, ((Pool<string>)pools.Get<string>(new OptionsPool(10, 10))).Capacity);
            Assert.AreEqual(10, pools.GetPool<string>().Capacity);

            pools.Add<ATag>(new OptionsPool(10, 10));

            Assert.AreEqual(10, pools.GetPool<ATag>(new OptionsPool(20, 20)).Capacity);
            Assert.AreEqual(10, ((ECS.Pool)pools.GetTag<BTag>(new OptionsPool(10, 10))).Capacity);
            Assert.AreEqual(10, pools.GetPool<BTag>().Capacity);
            Assert.Throws<InvalidCastException>(() => pools.Get<ATag>());
        }

        [Test]
        public void Contains()
        {
            var pools = new ECS.Pools(null);

            pools.Add<int>();
            pools.Add<ATag>();

            Assert.True(pools.Contains<int>());
            Assert.True(pools.Contains<ATag>());
            Assert.False(pools.Contains<string>());
            Assert.False(pools.Contains<BTag>());
        }

        [Test]
        public void Enumerable()
        {
            var pools = new ECS.Pools(null);

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