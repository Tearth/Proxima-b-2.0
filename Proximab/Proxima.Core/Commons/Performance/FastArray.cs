using System.Runtime.CompilerServices;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;

namespace Proxima.Core.Commons.Performance
{
    /// <summary>
    /// Represents a set of methods to fast array manipulating (emulating two-dimensional arrays as one-dimensional).
    /// </summary>
    /// <remarks>
    /// One-dimensional arrays are significantly faster because they use other set of CIL instructions,
    /// optimized to accessing and writing.
    /// </remarks>
    public class FastArray
    {
        /// <summary>
        /// Calculates a piece index (in bitboard pieces array).
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <returns>The piece index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetPieceIndex(Color color, PieceType pieceType)
        {
            return ((int)color * 6) + (int)pieceType;
        }

        /// <summary>
        /// Calculates a piece index (in Zobrist hash array).
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="field">The field index where the piece is located.</param>
        /// <returns>The piece index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetZobristPieceIndex(Color color, PieceType pieceType, int field)
        {
            return ((((int)color * 6) + (int)pieceType) << 6) + field;
        }

        /// <summary>
        /// Calculates a castling index (in castling array).
        /// </summary>
        /// <param name="color">The player color.</param>
        /// <param name="castlingType">The castling type.</param>
        /// <returns>The castling index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCastlingIndex(Color color, CastlingType castlingType)
        {
            return ((int)color << 1) + (int)castlingType;
        }

        /// <summary>
        /// Calculates a piece value based on position value arrays.
        /// </summary>
        /// <param name="gamePhase">The game phase.</param>
        /// <param name="pieceIndex">The piece index.</param>
        /// <returns>The index of value in position array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetEvaluationValueIndex(GamePhase gamePhase, int pieceIndex)
        {
            return ((int)gamePhase << 6) + pieceIndex;
        }
    }
}
