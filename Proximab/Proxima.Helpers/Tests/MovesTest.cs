using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using System;
using System.Diagnostics;

namespace Proxima.Helpers.Tests
{
    public class MovesTest
    {
        public MovesTest()
        {

        }

        public MovesTestData Run(Color initialColor, FriendlyBoard friendlyBoard, int depth, bool calculateEndNodes, bool verifyChecks)
        {
            var testData = new MovesTestData();
            var stopwatch = new Stopwatch();

            GC.Collect();

            stopwatch.Start();
            CalculateBitBoard(initialColor, new BitBoard(friendlyBoard), depth, calculateEndNodes, verifyChecks, testData);
            testData.Ticks = stopwatch.Elapsed.Ticks;

            return testData;
        }

        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, bool calculateEndNodes, bool verifyChecks, MovesTestData testData)
        {
            var enemyColor = ColorOperations.Invert(color);

            if (depth <= 0)
            {
                if(calculateEndNodes)
                {
                    bitBoard.Calculate(CalculationMode.OnlyAttacks);
                }

                testData.EndNodes++;
            }
            else
            {
                if(color == Color.White)
                {
                    bitBoard.Calculate(CalculationMode.WhiteMovesPlusAttacks);
                }
                else
                {
                    bitBoard.Calculate(CalculationMode.BlackMovesPlusAttacks);
                }

                var availableMoves = bitBoard.GetAvailableMoves();
                foreach (var move in availableMoves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    if (verifyChecks && bitBoardAfterMove.IsCheck(color))
                        continue;

                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, calculateEndNodes, verifyChecks, testData);
                }
            }

            testData.TotalNodes++;
        }
    }
}
