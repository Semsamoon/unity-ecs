using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class SparseArray
    {
        [Test]
        public void Constructor()
        {
            var sparseArray = new SparseArray<int>();
            var sparseArray10 = new SparseArray<int>(10);
            var sparseArray_10 = new SparseArray<int>(-10);

            Assert.AreEqual(10, sparseArray10.Length);
            Assert.Positive(sparseArray.Length);
            Assert.Positive(sparseArray_10.Length);
        }

        [Test]
        public void Getter()
        {
            var sparseArray = new SparseArray<int>();

            sparseArray[0] = 10;
            sparseArray[1] = 20;

            Assert.AreEqual(10, sparseArray[0]);
            Assert.AreEqual(20, sparseArray[1]);
            Assert.AreEqual(0, sparseArray[2]);
        }

        [Test]
        public void Extending()
        {
            var sparseArray = new SparseArray<int>(2);

            sparseArray[32] = 10;

            Assert.AreEqual(64, sparseArray.Length);
            Assert.AreEqual(10, sparseArray[32]);
            Assert.AreEqual(0, sparseArray[33]);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var sparseArray = new SparseArray<int>(8);

            for (var i = 0; i < 4; i++)
            {
                sparseArray[i] = i + 1;
            }

            var span = sparseArray.AsReadOnlySpan();

            Assert.AreEqual(8, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(i + 1, span[i]);
            }

            for (var i = 4; i < 8; i++)
            {
                Assert.AreEqual(0, span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var sparseArray = new SparseArray<int>(4);

            for (var i = 0; i < 4; i++)
            {
                sparseArray[i] = 10;
            }

            var j = 0;
            foreach (var value in sparseArray)
            {
                Assert.AreEqual(10, value);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}