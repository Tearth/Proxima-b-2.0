using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Proxima.Core.AI.HistoryHeuristic;
using Proxima.Core.AI.KillerHeuristic;
using Proxima.Core.AI.LazySMP;
using Proxima.Core.AI.Search;
using Proxima.Core.AI.Transposition;
using Proxima.Core.Boards;
using Proxima.Core.Commons.Colors;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AICore"/> class.
        /// </summary>
        public AICore()
        {
            _transpositionTable = new TranspositionTable();
        }

        /// <summary>
        /// Calculates the best possible move for the specified parameters.
        /// </summary>
        /// <param name="color">The initial player.</param>
        /// <param name="bitboard">The bitboard.</param>
        /// <param name="preferredTime">Time allocated for AI.</param>
        /// <returns>The result of AI calculating.</returns>
        public AIResult Calculate(Color color, Bitboard bitboard, float preferredTime, int helperTasks)
        {
            var result = new AIResult();
            var colorSign = ColorOperations.ToSign(color);
            var stopwatch = new Stopwatch();
            int estimatedTimeForNextIteration;

            var historyTable = new HistoryTable();
            var killerTable = new KillerTable();
            var regularSearch = new RegularSearch(_transpositionTable, historyTable, killerTable);

            result.Color = color;
            result.PreferredTime = preferredTime;

            var deadline = DateTime.Now.AddSeconds(preferredTime * 2).Ticks;

            historyTable.Clear();
            killerTable.Clear();

            if (bitboard.ReversibleMoves == 0 && preferredTime > 0)
            {
                _transpositionTable.Clear();
            }

            stopwatch.Start();
            do
            {
                result.Depth++;

                killerTable.SetInitialDepth(result.Depth);

                if (result.Depth >= AIConstants.MinimalDepthToStartHelperThreads)
                {
                    for (var i = 0; i < helperTasks; i++)
                    {
                        var param = new HelperTaskParameters
                        {
                            Bitboard = new Bitboard(bitboard),
                            Color = color,
                            Deadline = deadline,
                            InitialDepth = result.Depth
                        };

                        Task.Run(() => HelperTask(param));
                    }
                }

                var stats = new AIStats();
                var score = colorSign * regularSearch.Do(color, new Bitboard(bitboard), result.Depth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, deadline, false, stats);

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

        private void HelperTask(HelperTaskParameters param)
        {
            var historyTable = new HistoryTable();
            var killerTable = new KillerTable();
            var helperSearch = new RegularSearch(_transpositionTable, historyTable, killerTable);

            killerTable.SetInitialDepth(param.InitialDepth);

            helperSearch.Do(param.Color, param.Bitboard, param.InitialDepth, AIConstants.InitialAlphaValue, AIConstants.InitialBetaValue, param.Deadline, true, new AIStats());
        }

        /// <summary>
        /// Gets the list pf PV nodes for the specified bitboard and color. Nodes count is limited by
        /// <see cref="AIConstants.MaxDepth"/> value to avoid infinite repetitions.
        /// </summary>
        /// <param name="bitboard">The initial bitboard.</param>
        /// <param name="color">The initial color.</param>
        /// <returns>The list of PV nodes.</returns>
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
