namespace Proxima.Core.Evaluation.KingSafety
{
    public class KingSafetyResult
    {
        public int WhiteAttackedNeighbours { get; set; }
        public int BlackAttackedNeighbours { get; set; }

        public int Difference
        {
            get { return WhiteAttackedNeighbours - BlackAttackedNeighbours; }
        }
    }
}
