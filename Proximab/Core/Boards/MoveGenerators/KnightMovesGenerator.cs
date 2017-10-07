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
            return ((field & BitConstants.NotALine) << 17) |          
                   ((field & BitConstants.NotHLine) << 15) |        
                   ((field & BitConstants.NotALine & BitConstants.NotBLine) << 10) |        
                   ((field & BitConstants.NotGLine & BitConstants.NotHLine) << 6) |          
                   ((field & BitConstants.NotALine & BitConstants.NotBLine) >> 6) |      
                   ((field & BitConstants.NotGLine & BitConstants.NotHLine) >> 10) |       
                   ((field & BitConstants.NotALine) >> 15) |       
                   ((field & BitConstants.NotHLine) >> 17);
        }
    }
}
