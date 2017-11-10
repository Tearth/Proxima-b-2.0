using Proxima.Core.MoveGenerators.MagicBitboards.Keys;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    public static class MagicBitboardsContainer
    {
        public static ulong[] RookAttacks { get; private set; }
        public static ulong[] BishopAttacks { get; private set; }

        public static ulong[] RookKeys { get; private set; }
        public static ulong[] BishopKeys { get; private set; }
        
        public static void LoadKeys()
        {
            var keysLoader = new MagicKeysLoader();

            RookKeys = keysLoader.LoadRookKeys();
            BishopKeys = keysLoader.LoadBishopKeys();
        }

        public static void GenerateAttacks()
        {
            
        }
    }
}
