using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Sparse array is an <see cref="Array"/> with automatic extension.
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

        public SparseArray()
        {
            _array = new T[DefaultLength];
        }

        public SparseArray(int length)
        {
            length = Math.Max(length, 2);
            _array = new T[length];
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        private void ExtendTo(int index)
        {
            var length = Length;

            if (index < length)
            {
                return;
            }

            do
            {
                length *= 2;
            } while (index >= length);

            Array.Resize(ref _array, length);
        }
    }
}