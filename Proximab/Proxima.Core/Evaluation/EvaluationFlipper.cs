namespace Proxima.Core.Evaluation
{
    /// <summary>
    /// Represents a set of methods to manipulate on evaluation arrays.
    /// </summary>
    public static class EvaluationFlipper
    {
        /// <summary>
        /// Calculates evaluation values array for white player.
        /// </summary>
        /// <param name="array">The initial evaluation array.</param>
        /// <returns>The evaluation values array for white player.</returns>
        public static int[] CalculateWhiteArray(int[] array)
        {
            var flippedArray = new int[array.Length];

            for (int gamePhase = 0; gamePhase < 2; gamePhase++)
            {
                var gamePhaseOffset = gamePhase << 6;
                for (int field = 0; field < 64; field++)
                {
                    var flippedIndex = 64 - field - 1;
                    flippedArray[gamePhaseOffset + flippedIndex] = array[field];
                }
            }

            return flippedArray;
        }

        /// <summary>
        /// Calculates evaluation values array for black player.
        /// </summary>
        /// <param name="array">The initial evaluation array.</param>
        /// <returns>The evaluation values array for black player.</returns>
        public static int[] CalculateBlackArray(int[] array)
        {
            return FlipHorizontally(array);
        }

        /// <summary>
        /// Flips horizontally the specified array.
        /// </summary>
        /// <param name="array">The array to flip/</param>
        /// <returns>The flipped array.</returns>
        private static int[] FlipHorizontally(int[] array)
        {
            var flippedArray = new int[64];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    flippedArray[(y << 3) + (8 - x - 1)] = array[(y << 3) + x];
                }
            }

            return flippedArray;
        }
    }
}
