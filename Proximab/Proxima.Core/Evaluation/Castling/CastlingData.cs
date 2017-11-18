namespace Proxima.Core.Evaluation.Castling
{
    public class CastlingData
    {
        public int WhiteCastling { get; set; }
        public int BlackCastling { get; set; }

        public int Difference
        {
            get { return WhiteCastling - BlackCastling; }
        }
    }
}
