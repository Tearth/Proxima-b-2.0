using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem.Selections
{
    /// <summary>
    /// Represents information about the selection.
    /// </summary>
    internal class Selection
    {
        /// <summary>
        /// Gets the position of the selection (for board these will be values from 1 to 8).
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets the type of the selection.
        /// </summary>
        public SelectionType Type { get; private set; }

        public Selection()
        {
            
        }

        public Selection(Position position, SelectionType type)
        {
            Position = position;
            Type = type;
        }
    }
}
