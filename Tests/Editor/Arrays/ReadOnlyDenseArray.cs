using NUnit.Framework;

namespace ECS.Tests
{
    public sealed class ReadOnlyDenseArray
    {
        [Test]
        public void Constructor()
        {
            var denseArray = new DenseArray<int>();
            var readOnlyDenseArray = new ReadOnlyDenseArray<int>(denseArray);

            Assert.AreEqual(denseArray.Capacity, readOnlyDenseArray.Capacity);
            Assert.AreEqual(denseArray.Length, readOnlyDenseArray.Length);
        }

        [Test]
        public void Getter()
        {
            var denseArray = new DenseArray<int>();
            var readOnlyDenseArray = new ReadOnlyDenseArray<int>(denseArray);

            denseArray
                .Add(10)
                .Add(20);

            Assert.AreEqual(10, readOnlyDenseArray[0]);
            Assert.AreEqual(20, readOnlyDenseArray[1]);
            Assert.AreEqual(0, readOnlyDenseArray[2]);
        }

        [Test]
        public void Cast()
        {
            var readOnlyDenseArray = new ReadOnlyDenseArray<int>(new DenseArray<int>());
            var denseArray = (DenseArray<int>)readOnlyDenseArray;

            denseArray
                .Add(10)
                .Add(20);

            Assert.AreEqual(2, readOnlyDenseArray.Length);
            Assert.AreEqual(10, readOnlyDenseArray[0]);
            Assert.AreEqual(20, readOnlyDenseArray[1]);
            Assert.AreEqual(0, readOnlyDenseArray[2]);
        }

        [Test]
        public void ReadOnlySpan()
        {
            var denseArray = new DenseArray<int>();
            var readOnlyDenseArray = new ReadOnlyDenseArray<int>(denseArray);

            for (var i = 0; i < 4; i++)
            {
                denseArray.Add(i + 1);
            }

            var span = readOnlyDenseArray.AsReadOnlySpan();

            Assert.AreEqual(4, readOnlyDenseArray.Length);

            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(i + 1, span[i]);
            }
        }

        [Test]
        public void Enumerable()
        {
            var denseArray = new DenseArray<int>();
            var readOnlyDenseArray = new ReadOnlyDenseArray<int>(denseArray);

            for (var i = 0; i < 4; i++)
            {
                denseArray.Add(10);
            }

            var j = 0;
            foreach (var value in readOnlyDenseArray)
            {
                Assert.AreEqual(10, value);
                j++;
            }

            Assert.AreEqual(4, j);
        }
    }
}