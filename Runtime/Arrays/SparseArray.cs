using System;

namespace ECS
{
    public sealed class SparseArray<T>
    {
        private const int DefaultCapacity = 64;

        private T[] _array;

        public int Capacity { get; private set; }

        public T this[int index]
        {
            get => _array[index];
            set
            {
                if (Capacity <= index)
                {
                    ResizeForIndex(index);
                }

                _array[index] = value;
            }
        }

        public SparseArray(int capacity = DefaultCapacity)
        {
            _array = new T[capacity];
            Capacity = capacity;
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan();
        }

        private void ResizeForIndex(int index)
        {
            do
            {
                Capacity *= 2;
            } while (index >= Capacity);

            Array.Resize(ref _array, Capacity);
        }
    }
}