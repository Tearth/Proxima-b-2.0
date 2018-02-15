using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.KillerHeuristic
{
    /// <summary>
    /// Represents a set of methods to manage killer table heuristic.
    /// </summary>
    public class KillerTable
    {
        private Move[][][] _table;
        private int _initialDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KillerTable"/> class.
        /// </summary>
        public KillerTable()
        {
            _table = new Move[2][][];

            for (var color = 0; color < 2; color++)
            {
                _table[color] = new Move[AIConstants.MaxDepth][];

                for (var move = 0; move < AIConstants.MaxDepth; move++)
                {
                    _table[color][move] = new Move[AIConstants.KillerHeuristicSlotsCount];
                }
            }
        }

        /// <summary>
        /// Clears killer table (every killer move slot is set to null).
        /// </summary>
        public void Clear()
        {
            for (var color = 0; color < 2; color++)
            {
                for (var depth = 0; depth < AIConstants.MaxDepth; depth++)
                {
                    for (var slot = 0; slot < AIConstants.KillerHeuristicSlotsCount; slot++)
                    {
                        _table[color][depth][slot] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Sets initial depth. Must be called before next iterative deepening iteration.
        /// </summary>
        /// <param name="initialDepth">The initial depth to set.</param>
        public void SetInitialDepth(int initialDepth)
        {
            _initialDepth = initialDepth;
        }

        /// <summary>
        /// Adds the killer at the specified depth (if there is no available slot, other move is removed and new killer is inserted).
        /// </summary>
        /// <param name="depth">The killer move depth.</param>
        /// <param name="move">The killer move.</param>
        public void AddKiller(int depth, Move move)
        {
            if (!IsKiller(depth, move))
            {
                for (var slot = AIConstants.KillerHeuristicSlotsCount - 1; slot > 0; slot--)
                {
                    _table[(int)move.Color][_initialDepth - depth][slot] = _table[(int)move.Color][_initialDepth - depth][slot - 1];
                }

                _table[(int)move.Color][_initialDepth - depth][0] = move;
            }
        }

        /// <summary>
        /// Checks if the specified move is a killer move (has a potential to prune the rest of moves).
        /// </summary>
        /// <param name="depth">The potential killer move depth.</param>
        /// <param name="move">The potential killer move.</param>
        /// <returns>True if the specified move is a killer move, otherwise false.</returns>
        public bool IsKiller(int depth, Move move)
        {
            for (var slot = 0; slot < AIConstants.KillerHeuristicSlotsCount; slot++)
            {
                var moveToCheck = _table[(int)move.Color][_initialDepth - depth][slot];
                if (moveToCheck == null)
                {
                    return false;
                }

                if (move.From == moveToCheck.From && move.To == moveToCheck.To && move.Piece == moveToCheck.Piece)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
