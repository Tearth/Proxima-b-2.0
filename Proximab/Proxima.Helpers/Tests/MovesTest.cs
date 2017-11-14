using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators;
using Proxima.Helpers.Tests.Exceptions;
using System;
using System.Diagnostics;

namespace Proxima.Helpers.Tests
{
    public class MovesTest
    {
        public MovesTestData Run(Color initialColor, FriendlyBoard friendlyBoard, int depth, bool calculateEndNodes, bool verifyHash)
        {
            var testData = new MovesTestData();
            var stopwatch = new Stopwatch();

            GC.Collect();

            stopwatch.Start();
            CalculateBitBoard(initialColor, new BitBoard(friendlyBoard), depth, calculateEndNodes, verifyHash, testData);
            testData.Ticks = stopwatch.Elapsed.Ticks;

            return testData;
        }

        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, bool calculateEndNodes, bool verifyHash, MovesTestData testData)
        {
            if (verifyHash && bitBoard.GetHash(false) != bitBoard.GetHash(true))
            {
                testData.HashCorrect = false;
            }

            if (depth <= 0)
            {
                if(calculateEndNodes)
                {
                    bitBoard.Calculate(GeneratorMode.CalculateAttacks, GeneratorMode.CalculateAttacks);
                    bitBoard.GetEvaluation();
                }
                
                testData.EndNodes++;
            }
            else
            {
                var enemyColor = ColorOperations.Invert(color);
                var whiteMode = color == Color.White ? GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks : GeneratorMode.CalculateAttacks;
                var blackMode = color == Color.Black ? GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks : GeneratorMode.CalculateAttacks;

                bitBoard.Calculate(whiteMode, blackMode);

                var availableMoves = bitBoard.GetAvailableMoves();
                foreach (var move in availableMoves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    if (bitBoardAfterMove.IsCheck(color))
                    {
                        continue;
                    }

                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, calculateEndNodes, verifyHash, testData);
                }
            }

            testData.TotalNodes++;
        }
    }
}
