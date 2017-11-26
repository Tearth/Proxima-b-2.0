using System.Collections.Generic;

namespace Proxima.Core.Helpers.Bidirectional
{
    /// <summary>
    /// Represents an indexer for the bidirectional dictionary.
    /// </summary>
    /// <typeparam name="T1">The type of the first member.</typeparam>
    /// <typeparam name="T2">The type of the second member.</typeparam>
    public class BidirectionalIndexer<T1, T2>
    {
        private Dictionary<T1, T2> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalIndexer{T1, T2}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary associated with the indexer.</param>
        public BidirectionalIndexer(Dictionary<T1, T2> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="index">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public T2 this[T1 index]
        {
            get { return _dictionary[index]; }
            set { _dictionary[index] = value; }
        }

        /// <summary>
        /// Checks if the specified key is stored in the dictionary.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the specified key is available, otherwise false.</returns>
        public bool ContainsKey(T1 key)
        {
            return _dictionary.ContainsKey(key);
        }
    }
}
