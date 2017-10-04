using Core.Commons;

namespace GUI.Source.BoardSubsystem.Selections
{
    internal class Selection
    {
        public Position Position { get; set; }
        public SelectionType Type { get; set; }

        public Selection(Position position, SelectionType type)
        {
            Position = position;
            Type = type;
        }
    }
}
