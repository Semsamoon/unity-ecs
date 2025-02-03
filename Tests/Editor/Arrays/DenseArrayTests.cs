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

            Assert.AreEqual(0, new DenseArray<int>().Length);
        }

        [Test]
        public void SetterAndGetterAreCorrect()
        {
            var denseArray = new DenseArray<int>(2);
            denseArray.Add(10);
            denseArray[0] = 20;

            Assert.AreEqual(20, denseArray[0]);
        }

        [Test]
        public void AddingIsCorrect()
        {
            var denseArray = new DenseArray<int>(2);
            denseArray.Add(10);

            Assert.AreEqual(1, denseArray.Length);
            Assert.AreEqual(10, denseArray[0]);
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var denseArray = new DenseArray<int>(2);
            Assert.AreEqual(2, denseArray.Capacity);

            denseArray.Add(10);
            denseArray.Add(20);
            Assert.AreEqual(2, denseArray.Capacity);

            denseArray.Add(30);
            Assert.AreEqual(4, denseArray.Capacity);
        }

        [Test]
        public void RemovingIsCorrect()
        {
            var denseArray = new DenseArray<int>(2);
            Assert.DoesNotThrow(() => denseArray.RemoveAt(0));

            denseArray.Add(10);
            Assert.DoesNotThrow(() => denseArray.RemoveAt(0));
            Assert.AreEqual(0, denseArray.Length);
        }

        [Test]
        public void RemovingUsesBackSwap()
        {
            var denseArray = new DenseArray<int>(4);
            denseArray.Add(10);
            denseArray.Add(20);
            denseArray.Add(30);

            denseArray.RemoveAt(0);
            Assert.AreEqual(30, denseArray[0]);
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var denseArray = new DenseArray<int>(4);

            for (var i = 0; i < 3; i++)
            {
                denseArray.Add(10);
            }

            // Foreach-loop
            foreach (var i in denseArray.AsReadOnlySpan())
            {
                Assert.AreEqual(10, i);
            }

            for (var i = 0; i < 3; i++)
            {
                denseArray[i] = i;
            }

            var span = denseArray.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, span[i]);
            }
        }

        [Test]
        public void EnumerableIsCorrect()
        {
            var denseArray = new DenseArray<int>(4);
            denseArray.Add(10);
            denseArray.Add(10);
            denseArray.Add(10);

            foreach (var i in denseArray)
            {
                Assert.AreEqual(10, i);
            }
        }
    }
}