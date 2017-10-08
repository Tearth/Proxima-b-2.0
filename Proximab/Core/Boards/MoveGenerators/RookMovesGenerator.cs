using System;

namespace Core.Boards.MoveGenerators
{
    public class RookMovesGenerator
    {
        public RookMovesGenerator()
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
            byte initialIndex = (byte)Math.Pow(2, field);

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
