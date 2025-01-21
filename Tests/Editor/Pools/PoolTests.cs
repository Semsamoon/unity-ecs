using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class PoolTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(1, new Pool(10, 10).Length);
            Assert.DoesNotThrow(() => new Pool(-10, -10));

            Assert.AreEqual(1, new Pool<int>(10, 10).Length);
            Assert.DoesNotThrow(() => new Pool<int>(-10, -10));

            // Initial length must be 1
            Assert.AreEqual(1, new Pool().Length);
            Assert.AreEqual(1, new Pool<int>().Length);
        }

        [Test]
        public void SetterAndGetterAreCorrect()
        {
            var pool1 = new Pool(10);
            pool1.Add(new Entity(1, 0));
            Assert.AreEqual(new Entity(1, 0), pool1[1]);

            var pool2 = new Pool<int>(4, 4);
            pool2.AddOrSet(new Entity(1, 0), 10);
            pool2[1].item = 20;
            Assert.AreEqual(20, pool2[1].item);
        }

        [Test]
        public void AddingIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            pool1.Add(new Entity(1, 0));

            Assert.AreEqual(2, pool1.Length);
            Assert.AreEqual(new Entity(1, 0), pool1[1]);

            var pool2 = new Pool<int>(4, 4);
            pool2.AddOrSet(new Entity(1, 0), 10);

            Assert.AreEqual(2, pool2.Length);
            Assert.AreEqual(10, pool2[1].item);
        }

        [Test]
        public void SettingIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            pool1.Add(new Entity(1, 0));
            pool1.Add(new Entity(1, 0));

            Assert.AreEqual(2, pool1.Length);
            Assert.AreEqual(new Entity(1, 0), pool1[1]);

            var pool2 = new Pool<int>(4, 4);
            pool2.AddOrSet(new Entity(1, 0), 10);
            pool2.AddOrSet(new Entity(1, 0), 20);

            Assert.AreEqual(2, pool2.Length);
            Assert.AreEqual(20, pool2[1].item);
        }

        [Test]
        public void ContainsIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            pool1.Add(new Entity(1, 0));

            Assert.True(pool1.Contains(new Entity(1, 0)));
            Assert.False(pool1.Contains(new Entity(2, 0)));

            var pool2 = new Pool<int>(4, 4);
            pool2.AddOrSet(new Entity(1, 0), 10);

            Assert.True(pool2.Contains(new Entity(1, 0)));
            Assert.False(pool2.Contains(new Entity(2, 0)));
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var pool1 = new Pool(1, 1);
            Assert.DoesNotThrow(() => pool1.Add(new Entity(1, 0)));
            Assert.DoesNotThrow(() => pool1.Add(new Entity(2, 0)));

            var pool2 = new Pool<int>(1, 1);
            Assert.DoesNotThrow(() => pool2.AddOrSet(new Entity(1, 0), 10));
            Assert.DoesNotThrow(() => pool2.AddOrSet(new Entity(2, 0), 10));
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var pool1 = new Pool(2, 2);
            Assert.DoesNotThrow(() => pool1.Remove(new Entity(1, 0)));

            pool1.Add(new Entity(1, 0));
            Assert.DoesNotThrow(() => pool1.Remove(new Entity(1, 0)));
            Assert.AreEqual(1, pool1.Length);

            var pool2 = new Pool<int>(2, 2);
            Assert.DoesNotThrow(() => pool2.Remove(new Entity(1, 0)));

            pool2.AddOrSet(new Entity(1, 0), 10);
            Assert.DoesNotThrow(() => pool2.Remove(new Entity(1, 0)));
            Assert.AreEqual(1, pool2.Length);
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var pool1 = new Pool(4, 4);
            var pool2 = new Pool<int>(4, 4);

            // Foreach-loop
            foreach (var i in pool1.AsReadOnlySpan())
            {
                Assert.AreEqual(new Entity(0, 0), i);
            }

            foreach (var i in pool2.AsReadOnlySpan())
            {
                Assert.AreEqual((new Entity(0, 0), 0), i);
            }

            for (var i = 1; i < 3; i++)
            {
                pool1.Add(new Entity(i, i));
                pool2.AddOrSet(new Entity(i, i), i);
            }

            var span1 = pool1.AsReadOnlySpan();
            var span2 = pool2.AsReadOnlySpan();

            // For-loop
            for (var i = 1; i < 3; i++)
            {
                Assert.AreEqual(new Entity(i, i), span1[i]);
                Assert.AreEqual((new Entity(i, i), i), span2[i]);
            }
        }
    }
}