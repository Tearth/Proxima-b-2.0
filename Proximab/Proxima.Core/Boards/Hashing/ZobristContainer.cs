using Proxima.Core.Commons.Randoms;

namespace Proxima.Core.Boards.Hashing
{
    /// <summary>
    /// Represents a container of Zobrist keys.
    /// </summary>
    public static class ZobristContainer
    {
        /// <summary>
        /// Gets the piece keys.
        /// </summary>
        public static ulong[] Pieces { get; private set; }

        /// <summary>
        /// Gets the castling keys.
        /// </summary>
        public static ulong[] Castling { get; private set; }

        /// <summary>
        /// Gets the en passant keys.
        /// </summary>
        public static ulong[] EnPassant { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="ZobristContainer"/> class.
        /// </summary>
        static ZobristContainer()
        {
            var random = new Random64();

            InitPieces(random);
            InitCastling(random);
            InitEnPassant(random);
        }

        /// <summary>
        /// Inits random piece keys.
        /// </summary>
        /// <param name="random">The generator of the 64-bit integers.</param>
        public static void InitPieces(Random64 random)
        {
            Pieces = new ulong[64 * 12];
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = random.Next();
            }
        }

        /// <summary>
        /// Inits random castling keys.
        /// </summary>
        /// <param name="random">The generator of the 64-bit integers.</param>
        public static void InitCastling(Random64 random)
        {
            Castling = new ulong[4];
            for (int i = 0; i < 4; i++)
            {
                Castling[i] = random.Next();
            }
        }

        /// <summary>
        /// Inits random en passant keys.
        /// </summary>
        /// <param name="random">The generator of the 64-bit integers.</param>
        public static void InitEnPassant(Random64 random)
        {
            EnPassant = new ulong[8];
            for (int i = 0; i < 8; i++)
            {
                EnPassant[i] = random.Next();
            }
        }
    }
}
