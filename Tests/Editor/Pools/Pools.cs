﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class Pools
    {
        [Test]
        public void Constructor()
        {
            var pools = new ECS.Pools(null);
            var pools10 = new ECS.Pools(null, 10);

            Assert.AreEqual(0, pools.Length);
            Assert.AreEqual(0, pools10.Length);
        }

        [Test]
        public void Add()
        {
            var pools = new ECS.Pools(null);

            pools
                .Add<int>()
                .Add<int>(10)
                .Add<string>(10);

            Assert.AreEqual(2, pools.Length);
            Assert.AreNotEqual(10, pools.Get<int>().Capacity);
            Assert.AreEqual(10, pools.GetPool<string>().Capacity);

            pools
                .Add<ATag>()
                .Add<ATag>(10)
                .Add<BTag>(10);

            Assert.AreEqual(4, pools.Length);
            Assert.AreNotEqual(10, pools.GetTag<ATag>().Capacity);
            Assert.AreEqual(10, pools.GetPool<BTag>().Capacity);
        }

        [Test]
        public void Get()
        {
            var pools = new ECS.Pools(null);

            pools.Add<int>(10);

            Assert.AreEqual(10, pools.GetPool<int>(20).Capacity);
            Assert.AreEqual(10, pools.Get<int>().Capacity);
            Assert.AreEqual(10, pools.Get<string>(10).Capacity);
            Assert.AreEqual(10, pools.GetUnchecked<string>().Capacity);

            pools.Add<ATag>(10);

            Assert.AreEqual(10, pools.GetPool<ATag>().Capacity);
            Assert.AreEqual(10, pools.GetTag<ATag>().Capacity);
            Assert.AreEqual(10, pools.GetTag<BTag>(10).Capacity);
            Assert.AreEqual(10, pools.GetTagUnchecked<BTag>().Capacity);

            Assert.Throws<InvalidCastException>(() => pools.Get<ATag>());
            Assert.Throws<KeyNotFoundException>(() => pools.GetPoolUnchecked(typeof(char)));
        }

        [Test]
        public void Contains()
        {
            var pools = new ECS.Pools(null);

            pools
                .Add<int>()
                .Add<ATag>();

            Assert.True(pools.Contains<int>());
            Assert.True(pools.Contains<ATag>());
            Assert.False(pools.Contains<string>());
            Assert.False(pools.Contains<BTag>());
        }

        [Test]
        public void Enumerable()
        {
            var pools = new ECS.Pools(null);

            pools
                .Add<int>()
                .Add<ATag>();

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