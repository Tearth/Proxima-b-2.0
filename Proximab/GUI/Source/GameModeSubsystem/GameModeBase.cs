using Core.Commons;
using GUI.Source.BoardSubsystem;
using GUI.Source.BoardSubsystem.Persistence;
using GUI.Source.ConsoleSubsystem;
using GUI.Source.ConsoleSubsystem.Parser;
using GUI.Source.InputSubsystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GUI.Source.GameModeSubsystem
{
    internal abstract class GameModeBase
    {
        protected ConsoleManager _consoleManager;
        protected Board _board;

        public GameModeBase(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _board = new Board();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _board.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            _board.Input(inputManager);
        }

        public virtual void Logic()
        {
            _board.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _board.Draw(spriteBatch);
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.AddPiece: { AddPiece(command); break; }
            }
        }

        void SaveBoard(Command command)
        {
            var boardWriter = new BoardWriter();
            var board = _board.GetFriendlyBoard();

            var path = $"Boards\\{command.GetArgument<string>(0)}.board";
            boardWriter.Write(path, board);
        }

        void LoadBoard(Command command)
        {
            var boardReader = new BoardReader();
            var path = $"Boards\\{command.GetArgument<string>(0)}.board";

            if (!boardReader.BoardExists(path))
            {
                _consoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            _board.SetFriendlyBoard(boardReader.Read(path));
        }

        void AddPiece(Command command)
        {
            var piece = command.GetArgument<string>(0);
            var field = command.GetArgument<string>(1);

            var pieceType = PieceType.None;
            var pieceTypeParseResult = Enum.TryParse(piece, true, out pieceType);

            if(!pieceTypeParseResult)
            {
                _consoleManager.WriteLine($"$rInvalid piece type ($R{piece}$r)");
                return;
            }

            var fieldPosition = PositionConverter.ToPosition(field);

            if (fieldPosition == null)
            {
                _consoleManager.WriteLine($"$rInvalid field ($R{field}$r)");
                return;
            }

            _board.AddPiece(fieldPosition, pieceType);
        }
    }
}
