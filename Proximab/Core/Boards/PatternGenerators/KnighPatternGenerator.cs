namespace Core.Boards.PatternGenerators
{
    public class KnighPatternGenerator
    {
        public KnighPatternGenerator()
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
            return ((field & ~BitConstants.AFile)                       << 17) |          
                   ((field & ~BitConstants.HFile)                       << 15) |        
                   ((field & ~BitConstants.AFile & ~BitConstants.BFile) << 10) |        
                   ((field & ~BitConstants.GFile & ~BitConstants.HFile) << 6 ) |          
                   ((field & ~BitConstants.AFile & ~BitConstants.BFile) >> 6)  |      
                   ((field & ~BitConstants.GFile & ~BitConstants.HFile) >> 10) |       
                   ((field & ~BitConstants.AFile)                       >> 15) |       
                   ((field & ~BitConstants.HFile)                       >> 17);
        }
    }
}
