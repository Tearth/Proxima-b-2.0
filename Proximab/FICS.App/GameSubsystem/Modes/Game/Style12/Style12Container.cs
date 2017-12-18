using Proxima.Core.Commons.Colors;

namespace FICS.App.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents a container for Style12 FICS response.
    /// </summary>
    public class Style12Container
    {
        /// <summary>
        /// Gets or sets the style id (will be always "&#60;12&#62;").
        /// </summary>
        public string StyleID { get; set; }

        /// <summary>
        /// Gets or sets the current board state (each row means separate rank). First one is White's 8th rank, then 7th rank etc.
        /// </summary>
        public string[] BoardState { get; set; }

        /// <summary>
        /// Gets or sets the color of current player whose turn it is.
        /// </summary>
        public Color ColorToMove { get; set; }

        /// <summary>
        /// Gets the enemy color.
        /// </summary>
        public Color EnemyColor
        {
            get { return ColorOperations.Invert(ColorToMove); }
        }

        /// <summary>
        /// Gets or sets the double pawn push file (-1 if none).
        /// </summary>
        public int DoublePawnPush { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the white short castling is possible.
        /// </summary>
        public bool WhiteShortCastlingPossible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the white long castling is possible.
        /// </summary>
        public bool WhiteLongCastlingPossible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the black short castling is possible.
        /// </summary>
        public bool BlackShortCastlingPossible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the black long castling is possible.
        /// </summary>
        public bool BlackLongCastlingPossible { get; set; }

        /// <summary>
        /// Gets or sets the number of moves made since last irreversible move.
        /// </summary>
        public int Rule50Moves { get; set; }

        /// <summary>
        /// Gets or sets the game number (assigned by FICS).
        /// </summary>
        public int GameNumber { get; set; }

        /// <summary>
        /// Gets or sets the white's name.
        /// </summary>
        public string WhitePlayerName { get; set; }

        /// <summary>
        /// Gets or sets the black's name.
        /// </summary>
        public string BlackPlayerName { get; set; }

        /// <summary>
        /// Gets or sets the relation of engine to the game (-3 = isolated position, -2 = examiner, 
        /// -1 = move of the enemy, 1 = move of the engine, 0 = observing).
        /// </summary>
        public Style12RelationType Relation { get; set; }

        /// <summary>
        /// Gets or sets the initial time (in seconds).
        /// </summary>
        public int InitialTime { get; set; }

        /// <summary>
        /// Gets or sets the incremental time (in seconds).
        /// </summary>
        public int IncrementalTime { get; set; }

        /// <summary>
        /// Gets or sets the white material strength.
        /// </summary>
        public int WhiteMaterialStrength { get; set; }

        /// <summary>
        /// Gets or sets the black material strength.
        /// </summary>
        public int BlackMaterialStrength { get; set; }

        /// <summary>
        /// Gets or sets the remaining time of white.
        /// </summary>
        public int WhiteRemainingTime { get; set; }

        /// <summary>
        /// Gets or sets the remaining time of black.
        /// </summary>
        public int BlackRemainingTime { get; set; }

        /// <summary>
        /// Gets or sets the number of moves to made (will be always 1).
        /// </summary>
        public int MovesToMade { get; set; }

        /// <summary>
        /// Gets or sets the previous move notation (e2-e4, h8-b8 etc). None if this is the first move.
        /// </summary>
        public Style12Move PreviousMove { get; set; }

        /// <summary>
        /// Gets or sets the time taken to do the previous move.
        /// </summary>
        public string TimeOfPreviousMove { get; set; }

        /// <summary>
        /// Gets or sets the pretty previous move notation (e4, Nc6 etc). None if this is the first move.
        /// </summary>
        public string PrettyPreviousMoveNotation { get; set; }

        /// <summary>
        /// Gets or sets the board orientation (0 = white at bottom, 1 = black at bottom).
        /// </summary>
        public Style12OrientationType BoardOrientation { get; set; }
    }
}
