using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class DenseArrayTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(10, new DenseArray<int>(10).Capacity);
            Assert.Positive(new DenseArray<int>(-10).Capacity);
            Assert.Positive(new DenseArray<int>(0).Capacity);
            Assert.Positive(new DenseArray<int>().Capacity);

            Assert.AreEqual(10, new DenseArray<int, int>(10).Capacity);
            Assert.Positive(new DenseArray<int, int>(-10).Capacity);
            Assert.Positive(new DenseArray<int, int>(0).Capacity);
            Assert.Positive(new DenseArray<int, int>().Capacity);

            // Initial length must be 1
            Assert.AreEqual(1, new DenseArray<int>().Length);
            Assert.AreEqual(1, new DenseArray<int, int>().Length);
        }

        [Test]
        public void SetterAndGetterAreCorrect()
        {
            var denseArray1 = new DenseArray<int>(10);
            denseArray1.Add(10);
            denseArray1[1] = 20;
            Assert.AreEqual(20, denseArray1[1]);

            var denseArray2 = new DenseArray<int, int>(10);
            denseArray2.Add((10, 10));
            denseArray2[1] = (20, 20);
            Assert.AreEqual((20, 20), denseArray2[1]);
        }

        [Test]
        public void AddingIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(2);
            denseArray1.Add(10);

            Assert.AreEqual(2, denseArray1.Length);
            Assert.AreEqual(10, denseArray1[1]);

            var denseArray2 = new DenseArray<int, int>(2);
            denseArray2.Add((10, 10));

            Assert.AreEqual(2, denseArray2.Length);
            Assert.AreEqual((10, 10), denseArray2[1]);
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var denseArray1 = new DenseArray<int>(1);
            Assert.AreEqual(1, denseArray1.Capacity);

            denseArray1.Add(10);
            Assert.AreEqual(2, denseArray1.Capacity);

            var denseArray2 = new DenseArray<int, int>(1);
            Assert.AreEqual(1, denseArray2.Capacity);

            denseArray2.Add((10, 10));
            Assert.AreEqual(2, denseArray2.Capacity);
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(2);
            Assert.DoesNotThrow(() => denseArray1.RemoveAt(1));

            denseArray1.Add(10);
            Assert.DoesNotThrow(() => denseArray1.RemoveAt(1));
            Assert.AreEqual(1, denseArray1.Length);

            var denseArray2 = new DenseArray<int, int>(2);
            Assert.DoesNotThrow(() => denseArray2.RemoveAt(1));

            denseArray2.Add((10, 10));
            Assert.DoesNotThrow(() => denseArray2.RemoveAt(1));
            Assert.AreEqual(1, denseArray2.Length);
        }

        [Test]
        public void RemovingUsesBackSwap()
        {
            var denseArray1 = new DenseArray<int>(4);
            denseArray1.Add(10);
            denseArray1.Add(20);
            denseArray1.Add(30);

            denseArray1.RemoveAt(1);
            Assert.AreEqual(30, denseArray1[1]);

            var denseArray2 = new DenseArray<int, int>(4);
            denseArray2.Add((10, 10));
            denseArray2.Add((20, 20));
            denseArray2.Add((30, 30));

            denseArray2.RemoveAt(1);
            Assert.AreEqual((30, 30), denseArray2[1]);
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(4);
            var denseArray2 = new DenseArray<int, int>(4);

            // Foreach-loop
            foreach (var i in denseArray1.AsReadOnlySpan())
            {
                Assert.AreEqual(0, i);
            }

            foreach (var i in denseArray2.AsReadOnlySpan())
            {
                Assert.AreEqual((0, 0), i);
            }

            for (var i = 1; i < 3; i++)
            {
                denseArray1.Add(i);
                denseArray2.Add((i, i));
            }

            var span1 = denseArray1.AsReadOnlySpan();
            var span2 = denseArray2.AsReadOnlySpan();

            // For-loop
            for (var i = 1; i < 3; i++)
            {
                Assert.AreEqual(i, span1[i]);
                Assert.AreEqual((i, i), span2[i]);
            }
        }
    }
}