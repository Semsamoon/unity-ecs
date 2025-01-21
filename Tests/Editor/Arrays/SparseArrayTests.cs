using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class SparseArrayTests
    {
        [Test]
        public void ConstructorSetsValidValues()
        {
            Assert.AreEqual(10, new SparseArray<int>(10).Length);
            Assert.Positive(new SparseArray<int>(-10).Length);
            Assert.Positive(new SparseArray<int>(0).Length);
            Assert.Positive(new SparseArray<int>().Length);
        }

        [Test]
        public void SetterAndGetterAreCorrect()
        {
            var sparseArray = new SparseArray<int>(10);
            sparseArray[2] = 10;
            Assert.AreEqual(10, sparseArray[2]);
        }

        [Test]
        public void CanAutomaticallyExtend()
        {
            var sparseArray = new SparseArray<int>(2);

            // Can double once
            Assert.DoesNotThrow(() => sparseArray[2] = 10);
            Assert.AreEqual(4, sparseArray.Length);
            Assert.AreEqual(10, sparseArray[2]);

            // Can double more than one time
            Assert.DoesNotThrow(() => sparseArray[32] = 10);
            Assert.AreEqual(64, sparseArray.Length);
            Assert.AreEqual(10, sparseArray[32]);
        }

        [Test]
        public void ReadOnlySpanIsCorrect()
        {
            var sparseArray = new SparseArray<int>(2);

            // Foreach-loop
            foreach (var i in sparseArray.AsReadOnlySpan())
            {
                Assert.AreEqual(0, i);
            }

            for (var i = 0; i < 2; i++)
            {
                sparseArray[i] = i;
            }

            var span = sparseArray.AsReadOnlySpan();

            // For-loop
            for (var i = 0; i < 2; i++)
            {
                Assert.AreEqual(i, span[i]);
            }
        }
    }
}