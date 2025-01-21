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

            // Initial length must be 1
            Assert.AreEqual(1, new DenseArray<int>().Length);
        }

        [Test]
        public void SetterAndGetterAreCorrect()
        {
            var denseArray1 = new DenseArray<int>(10);
            denseArray1.Add(10);
            denseArray1[1] = 20;
            Assert.AreEqual(20, denseArray1[1]);
        }

        [Test]
        public void AddingIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(2);
            denseArray1.Add(10);

            Assert.AreEqual(2, denseArray1.Length);
            Assert.AreEqual(10, denseArray1[1]);
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var denseArray1 = new DenseArray<int>(1);
            Assert.AreEqual(1, denseArray1.Capacity);

            denseArray1.Add(10);
            Assert.AreEqual(2, denseArray1.Capacity);
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(2);
            Assert.DoesNotThrow(() => denseArray1.RemoveAt(1));

            denseArray1.Add(10);
            Assert.DoesNotThrow(() => denseArray1.RemoveAt(1));
            Assert.AreEqual(1, denseArray1.Length);
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
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var denseArray1 = new DenseArray<int>(4);

            // Foreach-loop
            foreach (var i in denseArray1.AsReadOnlySpan())
            {
                Assert.AreEqual(0, i);
            }

            for (var i = 1; i < 3; i++)
            {
                denseArray1.Add(i);
            }

            var span1 = denseArray1.AsReadOnlySpan();

            // For-loop
            for (var i = 1; i < 3; i++)
            {
                Assert.AreEqual(i, span1[i]);
            }
        }
    }
}