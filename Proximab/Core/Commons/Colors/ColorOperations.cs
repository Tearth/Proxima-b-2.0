namespace Core.Commons.Colors
{
    public static class ColorOperations
    {
        public static Color Invert(Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }

        public static Color GetPieceColor(PieceType pieceType)
        {
            var pieceIndex = (int)pieceType;

            if(pieceIndex >= (int)PieceType.WhitePawn && pieceIndex <= (int)PieceType.WhiteKing)
            {
                return Color.White;
            }
            else if(pieceIndex >= (int)PieceType.BlackPawn && pieceIndex <= (int)PieceType.BlackKing)
            {
                return Color.Black;
            }

            return Color.None;
        }
    }
}
