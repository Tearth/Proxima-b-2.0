using Proxima.Core.Commons.Randoms;

namespace Proxima.Core.Boards.Hashing
{
    public static class ZobristContainer
    {
        public static ulong[] Pieces;
        public static ulong[] Castling;
        public static ulong[] EnPassant;

        public static void Init()
        {
            var random = new Random64();

            InitPieces(random);
            InitCastling(random);
            InitEnPassant(random);
        }

        static void InitPieces(Random64 random)
        {
            Pieces = new ulong[64 * 12];
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = random.Next();
            }
        }

        static void InitCastling(Random64 random)
        {
            Castling = new ulong[4];
            for(int i=0; i<4; i++)
            {
                Castling[i] = random.Next();
            }
        }

        static void InitEnPassant(Random64 random)
        {
            EnPassant = new ulong[8];
            for(int i=0; i<8; i++)
            {
                EnPassant[i] = random.Next();
            }
        }
    }
}
