using System;
using System.Collections.Generic;

namespace ECS
{
    public readonly struct ReadOnlyDenseArray<T>
    {
        private readonly DenseArray<T> _array;

        public int Capacity => _array.Capacity;
        public int Length => _array.Length;

        public T this[int index] => _array[index];

        public ReadOnlyDenseArray(DenseArray<T> array)
        {
            _array = array;
        }

        public static explicit operator DenseArray<T>(ReadOnlyDenseArray<T> array)
        {
            return array._array;
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsReadOnlySpan();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _array.GetEnumerator();
        }
    }
}