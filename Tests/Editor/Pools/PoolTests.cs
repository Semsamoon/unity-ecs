using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class PoolTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(10, new Pool(10, 10).Capacity);
            Assert.Positive(new Pool(-10, -10).Capacity);
            Assert.AreEqual(0, new Pool().Length);
        }

        [Test]
        public void GettersAreCorrect()
        {
            var pool = new Pool(2, 2);
            pool.Add(new ECS.Entity(1, 0));
            Assert.AreEqual(new ECS.Entity(1, 0), pool[0]);
        }

        [Test]
        public void AddingIsCorrect()
        {
            var pool = new Pool(2, 2);
            pool.Add(new ECS.Entity(1, 0));

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(new ECS.Entity(1, 0), pool[0]);
        }

        [Test]
        public void SettingIsCorrect()
        {
            var pool = new Pool(2, 2);
            pool.Add(new ECS.Entity(1, 0));
            Assert.DoesNotThrow(() => pool.Add(new ECS.Entity(1, 0)));

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(new ECS.Entity(1, 0), pool[0]);
        }

        [Test]
        public void ContainsIsCorrect()
        {
            var pool = new Pool(2, 2);
            pool.Add(new ECS.Entity(1, 0));

            Assert.True(pool.Contains(new ECS.Entity(1, 0)));
            Assert.False(pool.Contains(new ECS.Entity(2, 0)));
            Assert.False(pool.Contains(new ECS.Entity()));
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var pool = new Pool(1, 1);
            Assert.DoesNotThrow(() => pool.Add(new ECS.Entity(1, 0)));
            Assert.DoesNotThrow(() => pool.Add(new ECS.Entity(2, 0)));
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var pool = new Pool(2, 2);
            Assert.DoesNotThrow(() => pool.Remove(new ECS.Entity(1, 0)));

            pool.Add(new ECS.Entity(1, 0));
            Assert.DoesNotThrow(() => pool.Remove(new ECS.Entity(1, 0)));
            Assert.AreEqual(0, pool.Length);
            Assert.DoesNotThrow(() => pool.Contains(new ECS.Entity(1, 0)));
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var pool = new Pool(4, 4);

            for (var i = 0; i < 3; i++)
            {
                pool.Add(new ECS.Entity(i + 1, 0));
            }

            // Foreach-loop
            var j = 0;

            foreach (var i in pool.AsReadOnlySpan())
            {
                Assert.AreEqual(new ECS.Entity(j + 1, 0), i);
                j++;
            }

            var span = pool.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(new ECS.Entity(i + 1, 0), span[i]);
            }
        }

        [Test]
        public void EnumerableIsCorrect()
        {
            var pool = new Pool(4, 4);
            pool.Add(new ECS.Entity(1, 0));
            pool.Add(new ECS.Entity(2, 0));
            pool.Add(new ECS.Entity(3, 0));

            var j = 0;

            foreach (var i in pool)
            {
                j++;
                Assert.AreEqual(new ECS.Entity(j, 0), i);
            }
        }
    }

    public sealed class PoolTests_T
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(10, new Pool<int>(10, 10).Capacity);
            Assert.Positive(new Pool<int>(-10, -10).Capacity);
            Assert.AreEqual(0, new Pool<int>().Length);
        }

        [Test]
        public void GettersAreCorrect()
        {
            var pool = new Pool<int>(2, 2);
            pool.Set(new ECS.Entity(1, 0), 10);
            Assert.AreEqual(new ECS.Entity(1, 0), pool[0].Entity);

            pool.Get(0) = 20;
            Assert.AreEqual(20, pool.Get(0));

            pool.Get(new ECS.Entity(1, 0)) = 30;
            Assert.AreEqual(30, pool.Get(new ECS.Entity(1, 0)));
        }

        [Test]
        public void AddingIsCorrect()
        {
            var pool = new Pool<int>(2, 2);
            pool.Set(new ECS.Entity(1, 0), 10);

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(10, pool.Get(0));
        }

        [Test]
        public void SettingIsCorrect()
        {
            var pool = new Pool<int>(2, 2);
            pool.Set(new ECS.Entity(1, 0), 10);
            Assert.DoesNotThrow(() => pool.Set(new ECS.Entity(1, 0), 20));

            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(20, pool.Get(0));
        }

        [Test]
        public void ContainsIsCorrect()
        {
            var pool = new Pool<int>(2, 2);
            pool.Set(new ECS.Entity(1, 0), 10);

            Assert.True(pool.Contains(new ECS.Entity(1, 0)));
            Assert.False(pool.Contains(new ECS.Entity(2, 0)));
            Assert.False(pool.Contains(new ECS.Entity()));
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var pool = new Pool<int>(1, 1);
            Assert.DoesNotThrow(() => pool.Set(new ECS.Entity(1, 0), 10));
            Assert.DoesNotThrow(() => pool.Set(new ECS.Entity(2, 0), 10));
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var pool = new Pool<int>(2, 2);
            Assert.DoesNotThrow(() => pool.Remove(new ECS.Entity(1, 0)));

            pool.Set(new ECS.Entity(1, 0), 10);
            Assert.DoesNotThrow(() => pool.Remove(new ECS.Entity(1, 0)));
            Assert.AreEqual(0, pool.Length);
            Assert.DoesNotThrow(() => pool.Contains(new ECS.Entity(1, 0)));
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var pool = new Pool<int>(4, 4);

            for (var i = 0; i < 3; i++)
            {
                pool.Set(new ECS.Entity(i + 1, 0), i);
            }

            // Foreach-loop
            var j = 0;

            foreach (var i in pool.AsReadOnlySpan())
            {
                Assert.AreEqual((new ECS.Entity(j + 1, 0), j), i);
                j++;
            }

            var span = pool.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual((new ECS.Entity(i + 1, 0), i), span[i]);
            }
        }

        [Test]
        public void EnumerableIsCorrect()
        {
            var pool = new Pool<int>(4, 4);
            pool.Set(new ECS.Entity(1, 0), 10);
            pool.Set(new ECS.Entity(2, 0), 20);
            pool.Set(new ECS.Entity(3, 0), 30);

            var j = 0;

            foreach (var i in pool)
            {
                j++;
                Assert.AreEqual((new ECS.Entity(j, 0), j * 10), i);
            }
        }
    }
}