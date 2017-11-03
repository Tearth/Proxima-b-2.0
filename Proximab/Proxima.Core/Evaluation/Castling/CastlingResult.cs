namespace Proxima.Core.Evaluation.Castling
{
    public class CastlingResult
    {
        public int WhiteCastling { get; set; }
        public int BlackCastling { get; set; }

        public int Difference
        {
            get { return WhiteCastling - BlackCastling; }
        }
    }
}
