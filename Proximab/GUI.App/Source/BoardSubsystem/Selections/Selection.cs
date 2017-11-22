using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem.Selections
{
    /// <summary>
    /// Represents information about the selection.
    /// </summary>
    internal class Selection
    {
        /// <summary>
        /// Gets the selection position (for board these will be values from 1 to 8).
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets selection type.
        /// </summary>
        public SelectionType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> class.
        /// </summary>
        /// <param name="position">The selection position</param>
        /// <param name="type">The selection type</param>
        public Selection(Position position, SelectionType type)
        {
            Position = position;
            Type = type;
        }
    }
}
