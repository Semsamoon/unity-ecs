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

            Assert.AreEqual(10, denseArray10.Capacity);
            Assert.Positive(denseArray.Capacity);

            Assert.AreEqual(0, denseArray.Length);
            Assert.AreEqual(0, denseArray10.Length);
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
        public void Set()
        {
            var denseArray = new DenseArray<int>();

            denseArray
                .Add(10)
                .Set(0, 20);

            Assert.AreEqual(20, denseArray[0]);
        }

        [Test]
        public void Extending()
        {
            var denseArray2 = new DenseArray<int>(2);

            for (var i = 0; i < 33; ++i)
            {
                denseArray2.Add(i);
            }

            Assert.AreEqual(64, denseArray2.Capacity);
            Assert.AreEqual(32, denseArray2[32]);
            Assert.AreEqual(0, denseArray2[33]);

            denseArray2.ExtendTo(256);

            Assert.AreEqual(512, denseArray2.Capacity);
        }

        [Test]
        public void Remove()
        {
            var denseArray = new DenseArray<int>();

            denseArray
                .Add(10)
                .RemoveAt(0);

            Assert.AreEqual(0, denseArray.Length);
        }

        [Test]
        public void BackSwap()
        {
            var denseArray = new DenseArray<int>();

            denseArray
                .Add(10)
                .Add(20)
                .Add(30)
                .RemoveAt(0);

            Assert.AreEqual(30, denseArray[0]);
            Assert.AreEqual(10, denseArray[2]);
        }

        [Test]
        public void Clear()
        {
            var denseArray = new DenseArray<int>();

            denseArray
                .Add(10)
                .Add(20)
                .Add(30)
                .Clear();

            Assert.AreEqual(0, denseArray.Length);
        }

        [Test]
        public void Swap()
        {
            var denseArray = new DenseArray<int>();

            denseArray
                .Add(10)
                .Add(20)
                .Add(30)
                .Swap(0, 2);

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