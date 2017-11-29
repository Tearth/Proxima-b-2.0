using Proxima.Core.Boards;
using Proxima.Core.MoveGenerators.MagicBitboards.Attacks;
using Proxima.Core.MoveGenerators.MagicBitboards.Keys;
using Proxima.Core.MoveGenerators.PatternGenerators;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    public static class MagicContainer
    {
        public static ulong[][] RookAttacks { get; private set; }
        public static ulong[][] BishopAttacks { get; private set; }

        public static ulong[] RookKeys { get; private set; }
        public static ulong[] BishopKeys { get; private set; }

        public static int[] RookMaskBitsCount { get; private set; }
        public static int[] BishopMaskBitsCount { get; private set; }

        public static void Init()
        {
            LoadKeys();
            GenerateAttacks();
        }

        private static void LoadKeys()
        {
            var keysLoader = new MagicKeysLoader();
            var maskBitsCountCalculator = new MaskBitsCountCalculator();

            RookKeys = keysLoader.LoadRookKeys();
            BishopKeys = keysLoader.LoadBishopKeys();

            RookMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.RookPattern);
            BishopMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.BishopPattern);
        }

        private static void GenerateAttacks()
        {
            var attacksParser = new AttacksParser();

            RookAttacks = attacksParser.ParseRookAttacks();
            BishopAttacks = attacksParser.ParseBishopAttacks();
        }

        public static ulong GetRookAttacks(int fieldIndex, ulong occupancy)
        {
            return GetAttacks(fieldIndex, occupancy, PatternsContainer.RookPattern, RookAttacks, RookKeys, RookMaskBitsCount);
        }

        public static ulong GetBishopAttacks(int fieldIndex, ulong occupancy)
        {
            return GetAttacks(fieldIndex, occupancy, PatternsContainer.BishopPattern, BishopAttacks, BishopKeys, BishopMaskBitsCount);
        }

        private static ulong GetAttacks(int fieldIndex, ulong occupancy, ulong[] patterns, ulong[][] attacks, ulong[] keys, int[] maskBitsCount)
        {
            var mask = patterns[fieldIndex];
            var key = keys[fieldIndex];

            var bitsCount = maskBitsCount[fieldIndex];
            var occupancyWithMask = occupancy & mask;

            var hash = (occupancyWithMask * key) >> (64 - bitsCount);
            return attacks[fieldIndex][hash];
        }
    }
}
