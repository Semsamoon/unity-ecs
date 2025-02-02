using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Represents sparse array.
    /// Consists of internal array that is extended automatically.<br/>
    /// <br/>
    /// <i>Sparse arrays are like standard arrays, but with automatic extension.
    /// In most cases they store indexes for <see cref="DenseArray{T}"/>,
    /// and indexes for them are <see cref="Entity"/> <see cref="Entity.Id"/>.</i>
    /// </summary>
    /// <typeparam name="T">Type of internal array</typeparam>
    public sealed class SparseArray<T>
    {
        private const int DefaultLength = 64;

        private T[] _array;

        /// <summary>
        /// Current length of internal array.
        /// Initialized in constructor and can not be increased manually.
        /// </summary>
        public int Length => _array.Length;

        /// <summary>
        /// Access to internal array.
        /// Extends internal array if <paramref name="index"/> is greater than or equals to array's <see cref="Length"/>.
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

        public SparseArray()
        {
            _array = new T[DefaultLength];
        }

        /// <param name="length">Initial <see cref="Length"/> of internal array</param>
        public SparseArray(int length)
        {
            if (length <= 0)
            {
                length = DefaultLength;
            }

            _array = new T[length];
        }

        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return _array.AsSpan();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_array.GetEnumerator();
        }

        /// <summary>
        /// Extends internal array to the specified <paramref name="index"/>.
        /// <see cref="Length"/> is increased 2 times until <paramref name="index"/> can be accessed.<br/>
        /// <br/>
        /// <i>Does not perform <see cref="Length"/> check before the first extenstion, so check it before calling this method.</i>
        /// </summary>
        /// <param name="index">Index that needs to be accessed</param>
        private void ExtendToIndex(int index)
        {
            var length = _array.Length;

            do
            {
                length *= 2;
            } while (index >= length);

            Array.Resize(ref _array, length);
        }
    }
}