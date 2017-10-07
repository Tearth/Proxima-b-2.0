namespace Core.Boards.MoveGenerators
{
    public class KnightMovesGenerator
    {
        public KnightMovesGenerator()
        {

        }

        public ulong[] Generate()
        {
            var predefinedMoves = new ulong[64];
            var bitPosition = 1ul;

            for(int i=0; i<64; i++)
            {
                predefinedMoves[i] = GetPattern(bitPosition);
                bitPosition = bitPosition << 1;
            }

            return predefinedMoves;
        }

        ulong GetPattern(ulong field)
        {
            return ((field & ~BitConstants.ALine) << 17) |          
                   ((field & ~BitConstants.HLine) << 15) |        
                   ((field & ~BitConstants.ALine & ~BitConstants.BLine) << 10) |        
                   ((field & ~BitConstants.GLine & ~BitConstants.HLine) << 6) |          
                   ((field & ~BitConstants.ALine & ~BitConstants.BLine) >> 6) |      
                   ((field & ~BitConstants.GLine & ~BitConstants.HLine) >> 10) |       
                   ((field & ~BitConstants.ALine) >> 15) |       
                   ((field & ~BitConstants.HLine) >> 17);
        }
    }
}
