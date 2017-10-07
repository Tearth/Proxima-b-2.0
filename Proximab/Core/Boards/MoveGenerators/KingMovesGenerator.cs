namespace Core.Boards.MoveGenerators
{
    public class KingMovesGenerator
    {
        public KingMovesGenerator()
        {

        }

        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];
            var bitPosition = 1ul;

            for (int i = 0; i < 64; i++)
            {
                predefinedMoves[i] = GetPattern(bitPosition);
                bitPosition = bitPosition << 1;
            }

            return predefinedMoves;
        }

        ulong GetPattern(ulong field)
        {
            return ((field & ~BitConstants.ALine) << 1) |
                   ((field & ~BitConstants.HLine) << 7) |
                   ( field                        << 8) |
                   ((field & ~BitConstants.ALine) << 9) |
                   ((field & ~BitConstants.HLine) >> 1) |
                   ((field & ~BitConstants.ALine) >> 7) |
                    (field                        >> 8) |
                   ((field & ~BitConstants.HLine) >> 9);
        }
    }
}
