using System;

namespace Proxima.Core.Boards
{
    public class CastlingData
    {
        public bool[] CastlingPossible { get; set; }

        public const int ShortCastling = 0;
        public const int LongCastling = 1;

        public CastlingData()
        {
            CastlingPossible = new bool[4];

            for(int i=0;i<CastlingPossible.Length; i++)
            {
                CastlingPossible[i] = true;
            }
        }

        public CastlingData(CastlingData castlingData)
        {
            CastlingPossible = new bool[4];

            Buffer.BlockCopy(castlingData.CastlingPossible, 0, CastlingPossible, 0, 
                             castlingData.CastlingPossible.Length * sizeof(bool));
        }
    }
}
