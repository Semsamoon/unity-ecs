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

            Assert.AreEqual(10, new Pool<int>(10, 10).Capacity);
            Assert.Positive(new Pool<int>(-10, -10).Capacity);
            Assert.AreEqual(0, new Pool<int>().Length);
        }

        [Test]
        public void GettersAreCorrect()
        {
            var pool1 = new Pool(2, 2);
            pool1.Add(new ECS.Entity(1, 0));
            Assert.AreEqual(new ECS.Entity(1, 0), pool1[0]);

            var pool2 = new Pool<int>(2, 2);
            pool2.Set(new ECS.Entity(1, 0), 10);
            Assert.AreEqual(new ECS.Entity(1, 0), pool2[0].Entity);

            pool2.Get(0) = 20;
            Assert.AreEqual(20, pool2.Get(0));

            pool2.Get(new ECS.Entity(1, 0)) = 30;
            Assert.AreEqual(30, pool2.Get(new ECS.Entity(1, 0)));
        }

        [Test]
        public void AddingIsCorrect()
        {
            var pool1 = new Pool(2, 2);
            pool1.Add(new ECS.Entity(1, 0));

            Assert.AreEqual(1, pool1.Length);
            Assert.AreEqual(new ECS.Entity(1, 0), pool1[0]);

            var pool2 = new Pool<int>(2, 2);
            pool2.Set(new ECS.Entity(1, 0), 10);

            Assert.AreEqual(1, pool2.Length);
            Assert.AreEqual(10, pool2.Get(0));
        }

        [Test]
        public void SettingIsCorrect()
        {
            var pool1 = new Pool(2, 2);
            pool1.Add(new ECS.Entity(1, 0));
            Assert.DoesNotThrow(() => pool1.Add(new ECS.Entity(1, 0)));

            Assert.AreEqual(1, pool1.Length);
            Assert.AreEqual(new ECS.Entity(1, 0), pool1[0]);

            var pool2 = new Pool<int>(2, 2);
            pool2.Set(new ECS.Entity(1, 0), 10);
            Assert.DoesNotThrow(() => pool2.Set(new ECS.Entity(1, 0), 20));

            Assert.AreEqual(1, pool2.Length);
            Assert.AreEqual(20, pool2.Get(0));
        }

        [Test]
        public void ContainsIsCorrect()
        {
            var pool1 = new Pool(2, 2);
            pool1.Add(new ECS.Entity(1, 0));

            Assert.True(pool1.Contains(new ECS.Entity(1, 0)));
            Assert.False(pool1.Contains(new ECS.Entity(2, 0)));
            Assert.False(pool1.Contains(new ECS.Entity()));

            var pool2 = new Pool<int>(2, 2);
            pool2.Set(new ECS.Entity(1, 0), 10);

            Assert.True(pool2.Contains(new ECS.Entity(1, 0)));
            Assert.False(pool2.Contains(new ECS.Entity(2, 0)));
            Assert.False(pool2.Contains(new ECS.Entity()));
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var pool1 = new Pool(1, 1);
            Assert.DoesNotThrow(() => pool1.Add(new ECS.Entity(1, 0)));
            Assert.DoesNotThrow(() => pool1.Add(new ECS.Entity(2, 0)));

            var pool2 = new Pool<int>(1, 1);
            Assert.DoesNotThrow(() => pool2.Set(new ECS.Entity(1, 0), 10));
            Assert.DoesNotThrow(() => pool2.Set(new ECS.Entity(2, 0), 10));
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var pool1 = new Pool(2, 2);
            Assert.DoesNotThrow(() => pool1.Remove(new ECS.Entity(1, 0)));

            pool1.Add(new ECS.Entity(1, 0));
            Assert.DoesNotThrow(() => pool1.Remove(new ECS.Entity(1, 0)));
            Assert.AreEqual(0, pool1.Length);
            Assert.DoesNotThrow(() => pool1.Contains(new ECS.Entity(1, 0)));

            var pool2 = new Pool<int>(2, 2);
            Assert.DoesNotThrow(() => pool2.Remove(new ECS.Entity(1, 0)));

            pool2.Set(new ECS.Entity(1, 0), 10);
            Assert.DoesNotThrow(() => pool2.Remove(new ECS.Entity(1, 0)));
            Assert.AreEqual(0, pool2.Length);
            Assert.DoesNotThrow(() => pool2.Contains(new ECS.Entity(1, 0)));
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            var pool2 = new Pool<int>(4, 4);

            for (var i = 0; i < 3; i++)
            {
                pool1.Add(new ECS.Entity(i + 1, 0));
                pool2.Set(new ECS.Entity(i + 1, 0), i);
            }

            // Foreach-loop
            var j = 0;

            foreach (var i in pool1.AsReadOnlySpan())
            {
                Assert.AreEqual(new ECS.Entity(j + 1, 0), i);
                j++;
            }

            j = 0;

            foreach (var i in pool2.AsReadOnlySpan())
            {
                Assert.AreEqual((new ECS.Entity(j + 1, 0), j), i);
                j++;
            }

            var span1 = pool1.AsReadOnlySpan();
            var span2 = pool2.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(new ECS.Entity(i + 1, 0), span1[i]);
                Assert.AreEqual((new ECS.Entity(i + 1, 0), i), span2[i]);
            }
        }

        [Test]
        public void EnumerableIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            pool1.Add(new ECS.Entity(1, 0));
            pool1.Add(new ECS.Entity(2, 0));
            pool1.Add(new ECS.Entity(3, 0));

            var j = 0;

            foreach (var i in pool1)
            {
                j++;
                Assert.AreEqual(new ECS.Entity(j, 0), i);
            }

            var pool2 = new Pool<int>(4, 4);
            pool2.Set(new ECS.Entity(1, 0), 10);
            pool2.Set(new ECS.Entity(2, 0), 20);
            pool2.Set(new ECS.Entity(3, 0), 30);

            j = 0;

            foreach (var i in pool2)
            {
                j++;
                Assert.AreEqual((new ECS.Entity(j, 0), j * 10), i);
            }
        }
    }
}