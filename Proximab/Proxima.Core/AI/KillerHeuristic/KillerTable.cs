using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI.KillerHeuristic
{
    public class KillerTable
    {
        private Move[][][] _table;
        private int _initialDepth;

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

        public void SetInitialDepth(int initialDepth)
        {
            _initialDepth = initialDepth;
        }

        public void SetKiller(Color color, int depth, Move move)
        {
            if (!IsKiller(color, depth, move))
            {
                for (var slot = AIConstants.KillerHeuristicSlotsCount - 1; slot > 0; slot--)
                {
                    _table[(int)color][_initialDepth - depth][slot] = _table[(int)color][_initialDepth - depth][slot - 1];
                }

                _table[(int)color][_initialDepth - depth][0] = move;
            }
        }

        public bool IsKiller(Color color, int depth, Move move)
        {
            for (var slot = 0; slot < AIConstants.KillerHeuristicSlotsCount; slot++)
            {
                var moveToCheck = _table[(int)color][_initialDepth - depth][slot];
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
