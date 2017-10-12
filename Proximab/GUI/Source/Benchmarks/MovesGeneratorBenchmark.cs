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

        public void Run(Color initialColor, BitBoard bitBoard, int depth)
        {
            var freshBitBoard = new BitBoard(bitBoard);
            var benchmarkData = new BenchmarkData();
            var startTime = DateTime.Now;

            CalculateBitBoard(initialColor, freshBitBoard, depth, ref benchmarkData);

            benchmarkData.Time = (float)(DateTime.Now - startTime).TotalSeconds;

            DisplayBenchmarkResult(benchmarkData);
        }

        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, ref BenchmarkData benchmarkData)
        {
            bitBoard.Calculate();
            var availableMoves = bitBoard.GetAvailableMoves(color);
            
            if(depth == 0)
            {
                benchmarkData.EndNodes += availableMoves.Count;
            }
            else
            {
                foreach(var move in availableMoves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    var enemyColor = ColorOperations.Invert(color);

                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, ref benchmarkData);
                }
            }

            benchmarkData.TotalNodes += availableMoves.Count;
        }

        void DisplayBenchmarkResult(BenchmarkData benchmarkData)
        {
            _consoleManager.WriteLine("$gBenchmark result:");
            _consoleManager.WriteLine($"$wTotal nodes: $g{benchmarkData.TotalNodes}");
            _consoleManager.WriteLine($"$wEnd nodes: $g{benchmarkData.EndNodes}");
            _consoleManager.WriteLine($"$wTime: $g{benchmarkData.Time}");
        }
    }
}
