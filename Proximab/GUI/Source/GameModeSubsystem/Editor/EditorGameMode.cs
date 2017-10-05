using Core.Boards;
using GUI.Source.BoardSubsystem;
using GUI.Source.ConsoleSubsystem;

namespace GUI.Source.GameModeSubsystem.Editor
{
    internal class EditorGameMode : GameModeBase
    {
        BitBoard _bitBoard;

        public EditorGameMode(ConsoleManager consoleManager) : base(consoleManager)
        {
            _bitBoard = new BitBoard();

            _board.OnFieldSelection += Board_OnFieldSelection;
            _board.OnPieceMove += Board_OnPieceMove;
        }

        void Board_OnFieldSelection(object sender, FieldSelectedEventArgs e)
        {
            
        }

        void Board_OnPieceMove(object sender, PieceMovedEventArgs e)
        {
            _board.MovePiece(e.From, e.To);

            _bitBoard.SyncWithFriendlyBoard(_board.GetFriendlyBoard());

            _board.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }
    }
}
