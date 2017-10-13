using Core.Boards;
using Core.Commons.Colors;
using GUI.Source.ConsoleSubsystem;
using System;

namespace GUI.Source.Benchmarks
{
    internal class MovesGeneratorBenchmark
    {
        ConsoleManager _consoleManager;

        public MovesGeneratorBenchmark(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
        }

        public void Run(Color initialColor, BitBoard bitBoard, int depth, bool verifyChecks)
        {
            var freshBitBoard = new BitBoard(bitBoard);
            var benchmarkData = new BenchmarkData();
            var startTime = DateTime.Now;
            
            CalculateBitBoard(initialColor, freshBitBoard, depth - 1, verifyChecks, ref benchmarkData);

            benchmarkData.Time = (float)(DateTime.Now - startTime).TotalSeconds;

            DisplayBenchmarkResult(benchmarkData);
        }

        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, bool verifyChecks, ref BenchmarkData benchmarkData)
        {
            var enemyColor = ColorOperations.Invert(color);
            benchmarkData.TotalNodes++;
            
            if (depth == 0)
            {
                benchmarkData.EndNodes++;
            }
            else
            {
                bitBoard.Calculate();
                //if (bitBoard.IsCheck(enemyColor))
                //{
                //    return;
                //}

                var availableMoves = bitBoard.GetAvailableMoves(color);
                
                foreach (var move in availableMoves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, verifyChecks, ref benchmarkData);
                }
            }
        }

        void DisplayBenchmarkResult(BenchmarkData benchmarkData)
        {
            _consoleManager.WriteLine("$wBenchmark result:");
            _consoleManager.WriteLine($"$wTotal nodes: $g{benchmarkData.TotalNodes}");
            _consoleManager.WriteLine($"$wEnd nodes: $g{benchmarkData.EndNodes}");
            _consoleManager.WriteLine($"$wTime: $c{benchmarkData.Time}");
        }
    }
}
