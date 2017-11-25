using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyEnPassant
    {
        public Position? WhiteEnPassant { get; set; }
        public Position? BlackEnPassant { get; set; }

        public FriendlyEnPassant()
        {
        }

        public FriendlyEnPassant(Position whiteEnPassant, Position blackEnPassant)
        {
            WhiteEnPassant = whiteEnPassant;
            BlackEnPassant = blackEnPassant;
        }

        public FriendlyEnPassant(ulong[] enPassant)
        {
            WhiteEnPassant = GetEnPassantPosition(enPassant[0]);
            BlackEnPassant = GetEnPassantPosition(enPassant[1]);
        }

        private Position? GetEnPassantPosition(ulong enPassant)
        {
            if (enPassant == 0)
            {
                return null;
            }

            var lsb = BitOperations.GetLSB(enPassant);
            enPassant = BitOperations.PopLSB(enPassant);

            var bitIndex = BitOperations.GetBitIndex(lsb);

            return BitPositionConverter.ToPosition(bitIndex);
        }
    }
}
