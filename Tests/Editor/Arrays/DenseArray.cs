using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class DenseArray
    {
        [Test]
        public void Constructor()
        {
            var denseArray = new DenseArray<int>();
            var denseArray10 = new DenseArray<int>(10);
            var denseArray_10 = new DenseArray<int>(-10);

            Assert.AreEqual(10, denseArray10.Capacity);
            Assert.Positive(denseArray.Capacity);
            Assert.Positive(denseArray_10.Capacity);

            Assert.AreEqual(0, denseArray.Length);
            Assert.AreEqual(0, denseArray10.Length);
            Assert.AreEqual(0, denseArray_10.Length);
        }

        [Test]
        public void Getter()
        {
            var denseArray = new DenseArray<int>();

            denseArray[0] = 10;
            denseArray[1] = 20;

            Assert.AreEqual(10, denseArray[0]);
            Assert.AreEqual(20, denseArray[1]);
            Assert.AreEqual(0, denseArray[2]);
        }

        [Test]
        public void Add()
        {
            var denseArray = new DenseArray<int>();

            denseArray.Add(10);

            Assert.AreEqual(1, denseArray.Length);
            Assert.AreEqual(10, denseArray[0]);

            denseArray.Add(20);

            Assert.AreEqual(2, denseArray.Length);
            Assert.AreEqual(20, denseArray[1]);
        }

        [Test]
        public void Extending()
        {
            var denseArray = new DenseArray<int>(2);

            for (var i = 0; i < 33; ++i)
            {
                denseArray.Add(i);
            }

            Assert.AreEqual(64, denseArray.Capacity);
            Assert.AreEqual(32, denseArray[32]);
            Assert.AreEqual(0, denseArray[33]);
        }

        [Test]
        public void Remove()
        {
            var denseArray = new DenseArray<int>();

            Assert.DoesNotThrow(() => denseArray.RemoveAt(0));

            denseArray.Add(10);
            denseArray.RemoveAt(0);

            Assert.AreEqual(0, denseArray.Length);
        }

        [Test]
        public void BackSwap()
        {
            var denseArray = new DenseArray<int>();

            denseArray.Add(10);
            denseArray.Add(20);
            denseArray.Add(30);
            denseArray.RemoveAt(0);

            Assert.AreEqual(30, denseArray[0]);
            Assert.AreEqual(10, denseArray[2]);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var denseArray = new DenseArray<int>();

            for (var i = 0; i < 4; i++)
            {
                denseArray.Add(i + 1);
            }

            var span = denseArray.AsReadOnlySpan();

            Assert.AreEqual(4, span.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(i + 1, span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var denseArray = new DenseArray<int>();

            for (var i = 0; i < 4; i++)
            {
                denseArray.Add(10);
            }

            var j = 0;
            foreach (var value in denseArray)
            {
                Assert.AreEqual(10, value);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}