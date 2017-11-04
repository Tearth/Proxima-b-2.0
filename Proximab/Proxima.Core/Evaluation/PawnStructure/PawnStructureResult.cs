namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureResult
    {
        public int WhiteDoubledPawns { get; set; }
        public int BlackDoubledPawns { get; set; }

        public int WhiteIsolatedPawns { get; set; }
        public int BlackIsolatedPawns { get; set; }

        public int Difference
        {
            get
            {
                return (WhiteDoubledPawns - BlackDoubledPawns) + 
                       (WhiteIsolatedPawns - BlackIsolatedPawns);
            }
        }
    }
}
