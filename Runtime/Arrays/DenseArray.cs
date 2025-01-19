using System;

namespace ECS
{
    public sealed class DenseArray<T>
    {
        private const int DefaultCapacity = 64;

        private T[] _array;

        public int Capacity { get; private set; }
        public int Length { get; private set; }

        public ref T this[int index]
        {
            get
            {
                if (Length <= index)
                {
                    throw new IndexOutOfRangeException($"Index (= {index}) must be less than {nameof(Length)} (= {Length})");
                }

                return ref _array[index];
            }
        }

        public DenseArray(int capacity = DefaultCapacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be greater than 0");
            }

            _array = new T[capacity];
            Capacity = capacity;
            Length = 0;
        }

        public void Add(T item)
        {
            if (Capacity <= Length)
            {
                DoubleCapacity();
            }

            _array[Length++] = item;
        }

        public void RemoveAt(int index)
        {
            Length--;
            (_array[index], _array[Length]) = (_array[Length], _array[index]);
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan(0, Length);
        }

        private void DoubleCapacity()
        {
            Capacity *= 2;
            Array.Resize(ref _array, Capacity);
        }
    }
}