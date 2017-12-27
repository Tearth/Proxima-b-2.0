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
        public Color EnemyColor => ColorOperations.Invert(ColorToMove);

        /// <summary>
        /// Gets or sets the double pawn push file (-1 if none).
        /// </summary>
        public int DoublePawnPush { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the castling is possible. First index is color, second castling type.
        /// </summary>
        public bool[,] CastlingPossible { get; set; }

        /// <summary>
        /// Gets or sets the number of moves made since last irreversible move.
        /// </summary>
        public int Rule50Moves { get; set; }

        /// <summary>
        /// Gets or sets the game number (assigned by FICS).
        /// </summary>
        public int GameNumber { get; set; }

        /// <summary>
        /// Gets or sets the player name with the specified color id.
        /// </summary>
        public string[] PlayerName { get; set; }

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
        /// Gets or sets the material strength with the specified color id.
        /// </summary>
        public int[] MaterialStrength { get; set; }

        /// <summary>
        /// Gets or sets the remaining time with the specified color id.
        /// </summary>
        public int[] RemainingTime { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Style12Container"/> class.
        /// </summary>
        public Style12Container()
        {
            BoardState = new string[8];
            CastlingPossible = new bool[2, 2];
            PlayerName = new string[2];
            MaterialStrength = new int[2];
            RemainingTime = new int[2];
        }
    }
}
