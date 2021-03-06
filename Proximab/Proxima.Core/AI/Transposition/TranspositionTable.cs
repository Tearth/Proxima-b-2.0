﻿using System.Collections.Concurrent;

namespace Proxima.Core.AI.Transposition
{
    /// <summary>
    /// Represents a set of methods to manage transposition table.
    /// </summary>
    public class TranspositionTable
    {
        private ConcurrentDictionary<ulong, TranspositionNode> _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranspositionTable"/> class.
        /// </summary>
        public TranspositionTable()
        {
            _table = new ConcurrentDictionary<ulong, TranspositionNode>();
        }

        /// <summary>
        /// Adds (or updates) node with the specified hash.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="node">The node.</param>
        public void AddOrUpdate(ulong hash, TranspositionNode node)
        {
            if (!Exists(hash))
            {
                _table[hash] = node;
            }
            else
            {
                var oldNode = _table[hash];
                if (node.Depth >= oldNode.Depth)
                {
                    _table[hash] = node;
                }
            }
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
        /// Gets the node based on hash.
        /// </summary>
        /// <param name="hash">The bitboard hash.</param>
        /// <returns>The transposition node with the specified hash.</returns>
        public TranspositionNode Get(ulong hash)
        {
            return _table[hash];
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
