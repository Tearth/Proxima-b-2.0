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

        public static void LoadKeys()
        {
            var keysLoader = new MagicKeysLoader();
            var maskBitsCountCalculator = new MaskBitsCountCalculator();

            RookKeys = keysLoader.LoadRookKeys();
            BishopKeys = keysLoader.LoadBishopKeys();

            RookMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.RookPattern);
            BishopMaskBitsCount = maskBitsCountCalculator.Calculate(PatternsContainer.BishopPattern);
        }

        public static void GenerateAttacks()
        {
            var attacksParser = new AttacksParser();

            RookAttacks = attacksParser.ParseRookAttacks();
            BishopAttacks = attacksParser.ParseBishopAttacks();
        }

        public static ulong GetRookAttacks(int fieldIndex, ulong occupancy)
        {
            var mask = PatternsContainer.RookPattern[fieldIndex];
            var key = RookKeys[fieldIndex];

            var bitsCount = RookMaskBitsCount[fieldIndex];
            var occupancyWithMask = occupancy & mask;

            var hash = (occupancyWithMask * key) >> (64 - bitsCount);
            var attacks = RookAttacks[fieldIndex][hash];

            return attacks;
        }

        public static ulong GetBishopAttacks(int fieldIndex, ulong occupancy)
        {
            var mask = PatternsContainer.BishopPattern[fieldIndex];
            var key = BishopKeys[fieldIndex];

            var bitsCount = BishopMaskBitsCount[fieldIndex];
            var occupancyWithMask = occupancy & mask;

            var hash = (occupancyWithMask * key) >> (64 - bitsCount);
            var attacks = BishopAttacks[fieldIndex][hash];

            return attacks;
        }
    }
}
