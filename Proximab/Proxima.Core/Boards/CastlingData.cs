using System;

namespace Proxima.Core.Boards
{
    public class CastlingData
    {
        public bool[,] CastlingPossible { get; set; }

        public const int ShortCastling = 0;
        public const int LongCastling = 1;

        public CastlingData()
        {
            CastlingPossible = new bool[2, 2];

            CastlingPossible[0, 0] = true;
            CastlingPossible[1, 0] = true;
            CastlingPossible[0, 1] = true;
            CastlingPossible[1, 1] = true;
        }

        public CastlingData(CastlingData castlingData)
        {
            CastlingPossible = new bool[2, 2];

            Buffer.BlockCopy(castlingData.CastlingPossible, 0, CastlingPossible, 0, 
                             castlingData.CastlingPossible.Length * sizeof(bool));
        }
    }
}
