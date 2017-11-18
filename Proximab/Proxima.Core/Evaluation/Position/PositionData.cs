namespace Proxima.Core.Evaluation.Position
{
    public class PositionData
    {
        public int WhitePosition { get; set; }
        public int BlackPosition { get; set; }

        public int Difference
        {
            get { return WhitePosition - BlackPosition; }
        }
    }
}
