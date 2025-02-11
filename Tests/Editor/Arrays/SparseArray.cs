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
        public void Set()
        {
            var sparseArray = new SparseArray<int>();

            sparseArray
                .Set(0, 10)
                .Set(4, 20);

            Assert.AreEqual(10, sparseArray[0]);
            Assert.AreEqual(20, sparseArray[4]);
        }

        [Test]
        public void Extending()
        {
            var sparseArray2 = new SparseArray<int>(2);

            sparseArray2[32] = 10;

            Assert.AreEqual(64, sparseArray2.Length);
            Assert.AreEqual(10, sparseArray2[32]);
            Assert.AreEqual(0, sparseArray2[33]);

            sparseArray2.ExtendTo(256);

            Assert.AreEqual(512, sparseArray2.Length);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var sparseArray8 = new SparseArray<int>(8);

            for (var i = 0; i < 4; i++)
            {
                sparseArray8[i] = i + 1;
            }

            var span = sparseArray8.AsReadOnlySpan();

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
            var sparseArray4 = new SparseArray<int>(4);

            for (var i = 0; i < 4; i++)
            {
                sparseArray4[i] = 10;
            }

            var j = 0;
            foreach (var value in sparseArray4)
            {
                Assert.AreEqual(10, value);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}