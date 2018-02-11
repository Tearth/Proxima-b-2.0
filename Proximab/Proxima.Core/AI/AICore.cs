using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Proxima.Core.AI.HistoryHeuristic;
using Proxima.Core.AI.KillerHeuristic;
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
        private HistoryTable _historyTable;
        private KillerTable _killerTable;
        private RegularSearch _regularSearch;

        public AICore()
        {
            _transpositionTable = new TranspositionTable();
            _historyTable = new HistoryTable();
            _killerTable = new KillerTable();

            _regularSearch = new RegularSearch(_transpositionTable, _historyTable, _killerTable);
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

            result.Color = color;
            result.PreferredTime = preferredTime;

            var deadline = DateTime.Now.AddSeconds(preferredTime * 2).Ticks;

            _historyTable.Clear();
            if (bitboard.ReversibleMoves == 0 && preferredTime != 0)
            {
                _transpositionTable.Clear();
            }
            _killerTable.Clear();

            stopwatch.Start();
            do
            {
                result.Depth++;

                _killerTable.SetInitialDepth(result.Depth);

                var stats = new AIStats();
                var score = colorSign * _regularSearch.Do(color, new Bitboard(bitboard), result.Depth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, deadline, stats);

                if (DateTime.Now.Ticks <= deadline)
                {
                    result.PVNodes = GetPVNodes(bitboard, color);
                    result.Score = score;

                    OnThinkingOutput?.Invoke(this, new ThinkingOutputEventArgs(result));
                }
                else
                {
                    result.Depth--;
                    _transpositionTable.Clear();
                }

                result.Stats = stats;
                result.Ticks = stopwatch.Elapsed.Ticks;

                estimatedTimeForNextIteration = (int)stopwatch.Elapsed.TotalMilliseconds * result.Stats.BranchingFactor;
            }
            while (estimatedTimeForNextIteration < preferredTime * 1000 &&
                   result.Depth < AIConstants.MaxDepth &&
                   Math.Abs(result.Score) != AIConstants.MateValue);

            return result;
        }

        private PVNodesList GetPVNodes(Bitboard bitboard, Color color)
        {
            var pvNodes = new PVNodesList();
            var boardHash = bitboard.GetHashForColor(color);

            while (_transpositionTable.Exists(boardHash) && pvNodes.Count < AIConstants.MaxDepth)
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
