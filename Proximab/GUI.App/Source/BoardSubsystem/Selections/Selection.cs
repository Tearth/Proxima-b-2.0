using Proxima.Core.Commons.Positions;

namespace GUI.App.Source.BoardSubsystem.Selections
{
    internal class Selection
    {
        public Position Position { get; private set; }
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
