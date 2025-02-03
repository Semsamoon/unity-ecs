using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Dense array is a <see cref="List{T}"/> with swap removing
    /// (swap with the last element) and index access like in <see cref="Array"/>.
    /// </summary>
    public sealed class DenseArray<T>
    {
        private const int DefaultCapacity = 64;

        private T[] _array;

        public int Capacity => _array.Length;
        public int Length { get; private set; }

        public ref T this[int index]
        {
            get
            {
                ExtendTo(index);
                return ref _array[index];
            }
        }

        public DenseArray()
        {
            _array = new T[DefaultCapacity];
        }

        public DenseArray(int capacity)
        {
            capacity = Math.Max(capacity, 2);
            _array = new T[capacity];
        }

        public void Add(T item)
        {
            ExtendTo(Length);
            _array[Length++] = item;
        }

        public void RemoveAt(int index)
        {
            if (index >= Length)
            {
                return;
            }

            Length--;
            (_array[index], _array[Length]) = (_array[Length], _array[index]);
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array[..Length];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_array[..Length]).GetEnumerator();
        }

        private void ExtendTo(int index)
        {
            var capacity = Capacity;

            if (index < capacity)
            {
                return;
            }

            do
            {
                capacity *= 2;
            } while (index >= capacity);

            Array.Resize(ref _array, capacity);
        }
    }
}