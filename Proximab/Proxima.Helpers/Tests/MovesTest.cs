using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators;
using System.Diagnostics;

namespace Proxima.Helpers.Tests
{
    /// <summary>
    /// Class for testing speed of move generators and evaluation functions. 
    /// </summary>
    public class MovesTest
    {
        readonly Color InitialColor = Color.White;

        /// <summary>
        /// Runs moves test with specific board and depth (0 = one level, 1 = two levels, ...).
        /// If verifyIntegrity is false, then flag in returned MovesTestData object will be
        /// always true.
        /// </summary>
        public MovesTestData Run(FriendlyBoard friendlyBoard, int depth, bool calculateEndNodes, bool verifyIntegrity)
        {
            var testData = new MovesTestData();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            CalculateBitBoard(InitialColor, new BitBoard(friendlyBoard), depth, calculateEndNodes, verifyIntegrity, testData);
            testData.Ticks = stopwatch.Elapsed.Ticks;

            return testData;
        }

        /// <summary>
        /// Recursive method for calculating bitboard. If depth is equal or less than zero, then
        /// current node is the last and next CalculateBitBoard call will not be executed.
        /// </summary>
        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, bool calculateEndNodes, bool verifyIntegrity, MovesTestData testData)
        {
            if (verifyIntegrity && !bitBoard.VerifyIntegrity())
            {
                testData.Integrity = false;
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
                var whiteMode = GetGeneratorMode(color);
                var blackMode = GetGeneratorMode(enemyColor);

                bitBoard.Calculate(whiteMode, blackMode);
                
                foreach (var move in bitBoard.Moves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    if (bitBoardAfterMove.IsCheck(color))
                    {
                        continue;
                    }

                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, calculateEndNodes, verifyIntegrity, testData);
                }
            }

            testData.TotalNodes++;
        }

        /// <summary>
        /// Returns generator mode for the specific color. If current color and initial color are
        /// the same, then returned enum will have flags for calculating moves and attacks. Otherwise,
        /// it will have only attacks flag.
        /// </summary>
        GeneratorMode GetGeneratorMode(Color currentColor)
        {
            return currentColor == Color.White && currentColor == InitialColor ? 
                GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks : 
                GeneratorMode.CalculateAttacks;
        }
    }
}
