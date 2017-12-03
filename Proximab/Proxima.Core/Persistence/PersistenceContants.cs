namespace Proxima.Core.Persistence
{
    /// <summary>
    /// Represents a set of constants for internal board file format.
    /// </summary>
    public static class PersistenceContants
    {
        public const string BoardSection = "!Board";
        public const string CastlingSection = "!Castling";
        public const string EnPassantSection = "!EnPassant";

        public const string EmptyBoardField = "--";
        public const string NullValue = "Null";
    }
}
