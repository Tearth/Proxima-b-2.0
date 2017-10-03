using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Selections;
using GUI.Source.ConsoleSubsystem;

namespace GUI.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _board.OnFieldSelection += Board_OnFieldSelection;
            _board.OnPieceMove += Board_OnPieceMove;
        }

        void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            
        }

        void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            
        }
    }
}
