using System;

namespace ECS
{
    /// <summary>
    /// Represents sparse array.
    /// Consists of internal array that is extended automatically.<br/>
    /// <br/>
    /// <i>Sparse arrays are like standard arrays, but with automatic extension.
    /// In most cases they store indexes for dense arrays.</i>
    /// </summary>
    /// <typeparam name="T">Type of internal array</typeparam>
    public sealed class SparseArray<T>
    {
        private const int DefaultCapacity = 64;

        private T[] _array;

        /// <summary>
        /// Current length of internal array.
        /// Initialized in constructor and can not be increased manually.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Access to internal array.
        /// If index is bigger than array's length, getter returns default value and setter extends internal array.
        /// </summary>
        /// <param name="index">Index of array element</param>
        public T this[int index]
        {
            get => _array[index];
            set
            {
                if (Length <= index)
                {
                    ExtendToIndex(index);
                }

                _array[index] = value;
            }
        }

        /// <param name="length">Initial length of internal array</param>
        public SparseArray(int length = DefaultCapacity)
        {
            _array = new T[length];
            Length = length;
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan();
        }

        /// <summary>
        /// Extends internal array to the specified index.
        /// Lenght is increased 2 times until index can be accessed.<br/>
        /// <br/>
        /// <i>Does not perform length check before the first extenstion, so check it before calling this method.</i>
        /// </summary>
        /// <param name="index">Index that needs to be accessed</param>
        private void ExtendToIndex(int index)
        {
            do
            {
                Length *= 2;
            } while (index >= Length);

            Array.Resize(ref _array, Length);
        }
    }
}