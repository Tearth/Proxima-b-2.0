using Proxima.Core.MoveGenerators.MagicBitboards.Attacks;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    /// <summary>
    /// Represents a set of methods to access to magic bitboards.
    /// </summary>
    public static class MagicContainer
    {
        private static ulong[][] _rookMagicBitboards;
        private static ulong[][] _bishopMagicBitboards;

        private static ulong[] _rookKeys;
        private static ulong[] _bishopKeys;

        private static int[] _rookMaskBitsCount;
        private static int[] _bishopMaskBitsCount;

        /// <summary>
        /// Inits magic bitboards. Must be called before first use of any other class method.
        /// </summary>
        public static void Init()
        {
            LoadKeys();
            GenerateMagicBitboards();
        }
        
        /// <summary>
        /// Calculates a magic bitboard for rook.
        /// </summary>
        /// <param name="fieldIndex">The field index.</param>
        /// <param name="occupancy">The bitboard occupancy.</param>
        /// <returns>The bitboard with available attacks (where set bit means that the piece can move to this field).</returns>
        public static ulong GetRookAttacks(int fieldIndex, ulong occupancy)
        {
            return GetAttacks(fieldIndex, occupancy, PatternsContainer.RookPattern, _rookMagicBitboards, _rookKeys, _rookMaskBitsCount);
        }

        /// <summary>
        /// Calculates a magic bitboard for bishop.
        /// </summary>
        /// <param name="fieldIndex">The field index.</param>
        /// <param name="occupancy">The bitboard occupancy.</param>
        /// <returns>The bitboard with available attacks (where set bit means that the piece can move to this field).</returns>
        public static ulong GetBishopAttacks(int fieldIndex, ulong occupancy)
        {
            return GetAttacks(fieldIndex, occupancy, PatternsContainer.BishopPattern, _bishopMagicBitboards, _bishopKeys, _bishopMaskBitsCount);
        }

        /// <summary>
        /// Calculates a magic bitboard for the specified parameters.
        /// </summary>
        /// <param name="fieldIndex">The field index.</param>
        /// <param name="occupancy">The bitboard occupancy.</param>
        /// <param name="patterns">The array of piece patterns.</param>
        /// <param name="attacks">The array of magic bitboards.</param>
        /// <param name="keys">The array of magic keys.</param>
        /// <param name="maskBitsCount">The array of mask bits count.</param>
        /// <returns>The bitboard with available attacks (where set bit means that the piece can move to this field).</returns>
        private static ulong GetAttacks(int fieldIndex, ulong occupancy, ulong[] patterns, ulong[][] attacks, ulong[] keys, int[] maskBitsCount)
        {
            var mask = patterns[fieldIndex];
            var key = keys[fieldIndex];

            var bitsCount = maskBitsCount[fieldIndex];
            var occupancyWithMask = occupancy & mask;

            var hash = (occupancyWithMask * key) >> (64 - bitsCount);
            return attacks[fieldIndex][hash];
        }

        /// <summary>
        /// Loads magic keys from files.
        /// </summary>
        private static void LoadKeys()
        {
            var keysLoader = new MagicKeysLoader();
            var maskBitsCountCalculator = new MaskBitsCountCalculator();

            _rookKeys = keysLoader.LoadRookKeys();
            _bishopKeys = keysLoader.LoadBishopKeys();

            _rookMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.RookPattern);
            _bishopMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.BishopPattern);
        }

        /// <summary>
        /// Generates magic bitboards for rook and bishop.
        /// </summary>
        private static void GenerateMagicBitboards()
        {
            var attacksParser = new MagicBitboardsGenerator();

            _rookMagicBitboards = attacksParser.GenerateMagicBitboards(new RookAttacksGenerator(), _rookMaskBitsCount, _rookKeys);
            _bishopMagicBitboards = attacksParser.GenerateMagicBitboards(new BishopAttacksGenerator(), _bishopMaskBitsCount, _bishopKeys);
        }
    }
}
