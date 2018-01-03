using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Proxima.Core.AI.Search;
using Proxima.Core.AI.SEE;
using Proxima.Core.AI.Transposition;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Core class of Proxima b AI.
    /// </summary>
    public class AICore
    {
        /// <summary>
        /// The event triggered when there is new thinking output available.
        /// </summary>
        public event EventHandler<ThinkingOutputEventArgs> OnThinkingOutput;

        private TranspositionTable _transpositionTable;
        private RegularSearch _regularSearch;

        public AICore()
        {
            _transpositionTable = new TranspositionTable();
            _regularSearch = new RegularSearch(_transpositionTable);
        }

        /// <summary>
        /// Calculates the best possible move for the specified parameters.
        /// </summary>
        /// <param name="color">The initial player.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="preferredTime">Time allocated for AI.</param>
        /// <returns>The result of AI calculating.</returns>
        public AIResult Calculate(Color color, Bitboard bitboard, float preferredTime)
        {
            var result = new AIResult();
            var colorSign = ColorOperations.ToSign(color);
            var stopwatch = new Stopwatch();
            int estimatedTimeForNextIteration;

            result.PreferredTime = preferredTime;
            _transpositionTable.Clear();

            stopwatch.Start();
            do
            {
                result.Depth++;

                var stats = new AIStats();
                result.Score = colorSign * _regularSearch.Do(color, new Bitboard(bitboard), result.Depth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, stats);

                result.PVNodes = GetPVNodes(bitboard, color);
                result.Stats = stats;
                result.Ticks = stopwatch.Elapsed.Ticks;

                OnThinkingOutput?.Invoke(this, new ThinkingOutputEventArgs(result));

                estimatedTimeForNextIteration = (int)stopwatch.Elapsed.TotalMilliseconds * result.Stats.BranchingFactor;
            }
            while (estimatedTimeForNextIteration < preferredTime * 1000 && result.Depth < 20);

            return result;
        }

        private PVNodesList GetPVNodes(Bitboard bitboard, Color color)
        {
            var pvNodes = new PVNodesList();
            var boardHash = bitboard.GetHashForColor(color);

            while (_transpositionTable.Exists(boardHash) && pvNodes.Count < 20)
            {
                var pvNode = _transpositionTable.Get(boardHash);

                if (pvNode.BestMove == null)
                {
                    break;
                }

                pvNodes.Add(pvNode.BestMove);
                bitboard = bitboard.Move(pvNode.BestMove);

                color = ColorOperations.Invert(color);
                boardHash = bitboard.GetHashForColor(color);
            }

            return pvNodes;
        }
    }
}
