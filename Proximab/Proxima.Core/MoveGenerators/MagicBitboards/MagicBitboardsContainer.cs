using Proxima.Core.MoveGenerators.MagicBitboards.Generators;

namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    public static class MagicBitboardsContainer
    {
        public static ulong[] RookAttacks { get; private set; }
        public static ulong[] BishopAttacks { get; private set; }

        public static ulong[] RookKeys { get; private set; }
        public static ulong[] BishopKeys { get; private set; }
        
        public static void GenerateAttacks()
        {

        }
    }
}
