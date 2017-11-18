namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureData
    {
        public int WhiteDoubledPawns { get; set; }
        public int BlackDoubledPawns { get; set; }

        public int WhiteIsolatedPawns { get; set; }
        public int BlackIsolatedPawns { get; set; }

        public int WhitePawnChain { get; set; }
        public int BlackPawnChain { get; set; }

        public int Difference
        {
            get
            {
                return (WhiteDoubledPawns - BlackDoubledPawns) + 
                       (WhiteIsolatedPawns - BlackIsolatedPawns) + 
                       (WhitePawnChain - BlackPawnChain);
            }
        }
    }
}
