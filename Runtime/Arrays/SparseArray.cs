using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Represents a dynamically resizing sparse array that stores elements of type <typeparamref name="T"/>.
    /// </summary>
    public sealed class SparseArray<T>
    {
        private const int DefaultLength = 64;

        private T[] _array;

        public int Length => _array.Length;

        public ref T this[int index]
        {
            get
            {
                ExtendTo(index);
                return ref _array[index];
            }
        }

        public SparseArray() : this(DefaultLength)
        {
        }

        public SparseArray(int length)
        {
            _array = new T[length];
        }

        public SparseArray<T> Set(int index, T value)
        {
            ExtendTo(index);
            _array[index] = value;
            return this;
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        public SparseArray<T> ExtendTo(int index)
        {
            var length = Length;

            if (index < length)
            {
                return this;
            }

            do
            {
                length *= 2;
            } while (index >= length);

            Array.Resize(ref _array, length);
            return this;
        }
    }
}