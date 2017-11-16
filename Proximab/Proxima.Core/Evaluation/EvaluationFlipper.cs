namespace Proxima.Core.Evaluation
{
    public static class EvaluationFlipper
    {
        public static int[] CalculateWhiteArray(int[] array)
        {
            var flippedArray = new int[array.Length];

            for(int gamePhase=0; gamePhase<2; gamePhase++)
            {
                var gamePhaseOffset = gamePhase << 6;
                for(int field = 0; field<64; field++)
                {
                    var flippedIndex = 64 - field - 1;
                    flippedArray[gamePhaseOffset + flippedIndex] = array[field];
                }
            }

            return flippedArray;
        }

        public static int[] CalculateBlackArray(int[] array)
        {
            return FlipHorizontally(array);
        }

        static int[] FlipHorizontally(int[] array)
        {
            var flippedArray = new int[64];

            for(int x=0; x<8; x++)
            {
                for(int y=0;y<8; y++)
                {
                    flippedArray[(y << 3) + (8 - x - 1)] = array[(y << 3) + x];
                }
            }

            return flippedArray;
        }
    }
}
