namespace Proxima.Core.Evaluation.PawnStructure
{
    public class PawnStructureResult
    {
        public int WhiteDoublePawns { get; set; }
        public int BlackDoublePawns { get; set; }

        public int Difference
        {
            get { return WhiteDoublePawns - BlackDoublePawns; }
        }
    }
}
