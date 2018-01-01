namespace Proxima.Core.Evaluation
{
    /// <summary>
    /// Represents a set of methods to manipulate on evaluation arrays.
    /// </summary>
    public static class EvaluationFlipper
    {
        /// <summary>
        /// Calculates evaluation values array for white.
        /// </summary>
        /// <param name="array">The initial evaluation array.</param>
        /// <returns>The evaluation values array for white.</returns>
        public static int[][] CalculateWhiteArray(int[][] array)
        {
            var gamePhasesCount = array.GetLength(0);
            var flippedArray = new int[gamePhasesCount][];

            for (var gamePhase = 0; gamePhase < gamePhasesCount; gamePhase++)
            {
                flippedArray[gamePhase] = new int[64];

                for (var field = 0; field < 64; field++)
                {
                    var flippedIndex = 64 - field - 1;
                    flippedArray[gamePhase][flippedIndex] = array[gamePhase][field];
                }
            }

            return flippedArray;
        }

        /// <summary>
        /// Calculates evaluation values array for black.
        /// </summary>
        /// <param name="array">The initial evaluation array.</param>
        /// <returns>The evaluation values array for black.</returns>
        public static int[][] CalculateBlackArray(int[][] array)
        {
            return FlipHorizontally(array);
        }

        /// <summary>
        /// Flips horizontally the specified array.
        /// </summary>
        /// <param name="array">The array to flip/</param>
        /// <returns>The flipped array.</returns>
        private static int[][] FlipHorizontally(int[][] array)
        {
            var gamePhasesCount = array.GetLength(0);
            var flippedArray = new int[gamePhasesCount][];

            for (var gamePhase = 0; gamePhase < gamePhasesCount; gamePhase++)
            {
                flippedArray[gamePhase] = new int[64];

                for (var y = 0; y < 8; y++)
                {
                    for (var x = 0; x < 8; x++)
                    {
                        flippedArray[gamePhase][(y * 8) + (8 - x - 1)] = array[gamePhase][(y * 8) + x];
                    }
                }
            }

            return flippedArray;
        }
    }
}
