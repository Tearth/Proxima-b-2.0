using System.Collections.Generic;

namespace Proxima.Core.AI.Transposition
{
    /// <summary>
    /// Represents a set of methods to manage transposition table.
    /// </summary>
    public class TranspositionTable
    {
        private Dictionary<ulong, TranspositionNode> _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranspositionTable"/> class.
        /// </summary>
        public TranspositionTable()
        {
            _table = new Dictionary<ulong, TranspositionNode>();
        }

        /// <summary>
        /// Adds (or updates) node with the specified hash.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="node">The node.</param>
        public void AddOrUpdate(ulong hash, TranspositionNode node)
        {
            _table[hash] = node;
        }

        /// <summary>
        /// Checks if node with the specified hash exists in the transposition table.
        /// </summary>
        /// <param name="hash">The searched hash.</param>
        /// <returns>True if node with the specified hash exists, otherwise false.</returns>
        public bool Exists(ulong hash)
        {
            return _table.ContainsKey(hash);
        }

        /// <summary>
        /// Clears transposition table.
        /// </summary>
        public void Clear()
        {
            _table.Clear();
        }
    }
}
