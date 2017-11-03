namespace Proxima.Core.Evaluation
{
    public static class EvaluationConstants
    {
        //Material
        public const int PawnValue = 100;
        public const int RookValue = 500;
        public const int KnightValue = 300;
        public const int BishopValue = 300;
        public const int QueenValue = 800;
        public const int KingValue = 10000;

        //Mobility
        public const int MobilityRatio = 1;
        public const int MobilityBigCenterRatio = 2;
        public const int MobilitySmallCenterRatio = 4;

        //Castling
        public const int CastlingRatio = 10;
    }
}
