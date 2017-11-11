namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    public static class MagicConstants
    {
        public const int RookMovesBits = 12;
        public const int RookMovesPerField = 1 << RookMovesBits;

        public const int BishopMovesBits = 9;
        public const int BishopMovesPerField = 1 << BishopMovesBits;
    }
}
