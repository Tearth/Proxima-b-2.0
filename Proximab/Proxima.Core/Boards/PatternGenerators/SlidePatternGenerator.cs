using System;

namespace Proxima.Core.Boards.PatternGenerators
{
    public class SlidePatternGenerator
    {
        public SlidePatternGenerator()
        {

        }

        public byte[,] Generate()
        {
            var predefinedMoves = new byte[256, 8];

            for(int occupancy = 0; occupancy < 256; occupancy++)
            {
                for(int field = 0; field < 8; field++)
                {
                    predefinedMoves[occupancy, field] = GetAvailableMoves((byte)occupancy, (byte)field);
                }
            }

            return predefinedMoves;
        }

        byte GetAvailableMoves(byte occupancy, byte field)
        {
            byte availableMoves = 0;
            byte initialIndex = (byte)(1 << field);

            occupancy &= (byte)~initialIndex;

            var currentIndex = initialIndex;
            while(currentIndex != 0 && (occupancy & currentIndex) == 0)
            {
                currentIndex >>= 1;
                availableMoves |= currentIndex;
            }

            currentIndex = initialIndex;
            while (currentIndex != 0 && (occupancy & currentIndex) == 0)
            {
                currentIndex <<= 1;
                availableMoves |= currentIndex;
            }

            return availableMoves;
        }
    }
}
