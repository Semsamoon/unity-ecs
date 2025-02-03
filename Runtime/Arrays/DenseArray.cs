using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Represents dense array.
    /// Consists of internal array that is extended automatically.
    /// The first element in array is counted as invalid.<br/>
    /// <br/>
    /// <i>Dense arrays are like lists, but with back swap remove operation.
    /// In most cases they store components, and indexes for them are stored in <see cref="SparseArray{T}"/></i>
    /// </summary>
    /// <typeparam name="T">Type of internal array</typeparam>
    public sealed class DenseArray<T>
    {
        private const int DefaultCapacity = 64;

        private T[] _array;

        /// <summary>
        /// Current capacity of internal array.
        /// Initialized in constructor and can not be increased manually.
        /// </summary>
        public int Capacity => _array.Length;

        /// <summary>
        /// Current length of internal array.
        /// Initialized in constructor and can not be increased manually.<br/>
        /// <br/>
        /// <i>Remember that the first element is counted as invalid.</i>
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Access to internal array.<br/>
        /// <br/>
        /// <i>Remember that the first element is counted as invalid.</i>
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Throws if <paramref name="index"/> is greater than or equal to array's <see cref="Length"/>
        /// </exception>
        /// <param name="index">Index of array element</param>
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
            Length = 1;
        }

        /// <param name="capacity">Initial <see cref="Capacity"/> of internal array</param>
        public DenseArray(int capacity)
        {
            capacity = Math.Max(capacity, 2);
            _array = new T[capacity];
            Length = 1;
        }

        /// <summary>
        /// Adds <paramref name="item"/> to the end of internal array and extends it if needed.
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            ExtendTo(Length);
            _array[Length++] = item;
        }

        /// <summary>
        /// Removes element from internal array by specified <paramref name="index"/>.
        /// Uses back swap (a swap with the last element) to avoid array shifting.<br/>
        /// <br/>
        /// <i>Ensure that indexes in <see cref="SparseArray{T}"/> are correct after back swap.</i>
        /// </summary>
        /// <param name="index">Index of element to remove</param>
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