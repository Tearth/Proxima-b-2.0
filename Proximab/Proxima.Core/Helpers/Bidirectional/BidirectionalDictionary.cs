using System.Collections.Generic;

namespace Proxima.Core.Helpers.Bidirectional
{
    /// <summary>
    /// Represents a bidirectional dictionary (values can be accessed by the key and the value).
    /// </summary>
    /// <typeparam name="T1">The type of the first member.</typeparam>
    /// <typeparam name="T2">The type of the second member.</typeparam>
    public class BidirectionalDictionary<T1, T2>
    {
        /// <summary>
        /// Gets the dictionary with first member as the key and second member as the value.
        /// </summary>
        public BidirectionalIndexer<T1, T2> Forward { get; }

        /// <summary>
        /// Gets the reversed dictionary with second member as the key and first member as the value.
        /// </summary>
        public BidirectionalIndexer<T2, T1> Reverse { get; }

        private Dictionary<T1, T2> _forwardDictionary;
        private Dictionary<T2, T1> _reverseDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDictionary{T1, T2}"/> class.
        /// </summary>
        public BidirectionalDictionary()
        {
            _forwardDictionary = new Dictionary<T1, T2>();
            _reverseDictionary = new Dictionary<T2, T1>();

            Forward = new BidirectionalIndexer<T1, T2>(_forwardDictionary);
            Reverse = new BidirectionalIndexer<T2, T1>(_reverseDictionary);
        }

        /// <summary>
        /// Adds the specified values to the dictionary.
        /// </summary>
        /// <param name="firstMemberValue">The value of the first member.</param>
        /// <param name="secondMemberValue">The value of the second member.</param>
        public void Add(T1 firstMemberValue, T2 secondMemberValue)
        {
            _forwardDictionary.Add(firstMemberValue, secondMemberValue);
            _reverseDictionary.Add(secondMemberValue, firstMemberValue);
        }
    }
}
