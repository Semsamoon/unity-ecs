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
        /// Extends internal array if index is bigger than or equals to array's length.
        /// </summary>
        /// <param name="index">Index of array element</param>
        public ref T this[int index]
        {
            get
            {
                if (index >= Length)
                {
                    ExtendToIndex(index);
                }

                return ref _array[index];
            }
        }

        /// <param name="length">Initial length of internal array</param>
        public SparseArray(int length = DefaultCapacity)
        {
            if (length <= 0)
            {
                length = DefaultCapacity;
            }

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