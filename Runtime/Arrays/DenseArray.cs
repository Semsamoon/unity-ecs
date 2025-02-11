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

        public DenseArray() : this(DefaultCapacity)
        {
        }

        public DenseArray(int capacity)
        {
            capacity = capacity > 0 ? capacity : DefaultCapacity;
            _array = new T[capacity];
        }

        public DenseArray<T> Add(T item)
        {
            ExtendTo(Length);
            _array[Length++] = item;
            return this;
        }

        public DenseArray<T> Set(int index, T item)
        {
            ExtendTo(index);
            _array[index] = item;
            return this;
        }

        public DenseArray<T> RemoveAt(int index)
        {
            if (index >= Length)
            {
                return this;
            }

            Length--;
            Swap(index, Length);
            return this;
        }

        public DenseArray<T> Clear()
        {
            Length = 0;
            return this;
        }

        public DenseArray<T> Swap(int i, int j)
        {
            (_array[i], _array[j]) = (_array[j], _array[i]);
            return this;
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array[..Length];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_array[..Length]).GetEnumerator();
        }

        public DenseArray<T> ExtendTo(int index)
        {
            var capacity = Capacity;

            if (index < capacity)
            {
                return this;
            }

            do
            {
                capacity *= 2;
            } while (index >= capacity);

            Array.Resize(ref _array, capacity);
            return this;
        }
    }
}