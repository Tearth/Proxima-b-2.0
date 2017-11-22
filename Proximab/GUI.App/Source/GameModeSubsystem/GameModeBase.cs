using System;
using System.Collections.Generic;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.InputSubsystem;
using GUI.App.Source.PromotionSubsystem;
using GUI.ColorfulConsole;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.Friendly.Persistence;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;

namespace GUI.App.Source.GameModeSubsystem
{
    internal abstract class GameModeBase
    {
        protected ConsoleManager ConsoleManager { get; set; }
        protected PiecesProvider PiecesProvider { get; set; }

        protected VisualBoard VisualBoard { get; set; }
        protected PromotionWindow PromotionWindow { get; set; }

        protected BitBoard BitBoard { get; set; }

        public GameModeBase(ConsoleManager consoleManager)
        {
            ConsoleManager = consoleManager;
            ConsoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            PiecesProvider = new PiecesProvider();

            VisualBoard = new VisualBoard(PiecesProvider);
            PromotionWindow = new PromotionWindow(PiecesProvider);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            PiecesProvider.LoadContent(contentManager);
            VisualBoard.LoadContent(contentManager);
            PromotionWindow.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            if (!PromotionWindow.Active)
            {
                VisualBoard.Input(inputManager);
            }

            PromotionWindow.Input(inputManager);
        }

        public virtual void Logic()
        {
            VisualBoard.Logic();
            PromotionWindow.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            VisualBoard.Draw(spriteBatch);
            PromotionWindow.Draw(spriteBatch);
        }

        protected void CalculateBitBoard(FriendlyBoard friendlyBoard)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(friendlyBoard, mode, mode);
        }

        protected void CalculateBitBoard(Move move)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(move, mode, mode);
        }

        protected void CalculateBitBoard(FriendlyBoard friendlyBoard, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            BitBoard = new BitBoard(friendlyBoard);
            BitBoard.Calculate(whiteMode, blackMode);

            VisualBoard.FriendlyBoard = BitBoard.GetFriendlyBoard();
        }

        protected void CalculateBitBoard(Move move, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            BitBoard = BitBoard.Move(move);
            BitBoard.Calculate(whiteMode, blackMode);

            VisualBoard.FriendlyBoard = BitBoard.GetFriendlyBoard();
        }

        private void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.Attacks: { DrawAttacks(command); break; }
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.Check: { DisplayCheckStatus(command); break; }
                case CommandType.Castling: { DisplayCastlingFlags(command); break; }
                case CommandType.Evaluation: { DisplayEvaluation(command); break; }
                case CommandType.Hash: { DisplayBoardHash(command); break; }
            }
        }

        private void DrawOccupancy(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> occupancy;
            if (colorArgument == "all")
            {
                occupancy = VisualBoard.FriendlyBoard.GetOccupancy();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    ConsoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                occupancy = VisualBoard.FriendlyBoard.GetOccupancy(colorType);
            }

            VisualBoard.AddExternalSelections(occupancy);
        }

        private void DrawAttacks(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> attacks;
            if (colorArgument == "all")
            {
                attacks = VisualBoard.FriendlyBoard.GetAttacks();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    ConsoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                attacks = VisualBoard.FriendlyBoard.GetAttacks(colorType);
            }

            VisualBoard.AddExternalSelections(attacks);
        }

        private void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = VisualBoard.FriendlyBoard;

            var path = $"Boards\\{boardNameArgument}.board";
            boardWriter.Write(path, board);
        }

        private void LoadBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardReader = new BoardReader();
            var path = $"Boards\\{boardNameArgument}.board";

            if (!boardReader.BoardExists(path))
            {
                ConsoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            CalculateBitBoard(boardReader.Read(path));
        }

        private void DisplayCheckStatus(Command command)
        {
            var whiteCheck = BitBoard.IsCheck(Color.White);
            var blackCheck = BitBoard.IsCheck(Color.Black);

            ConsoleManager.WriteLine($"$cWhite king checked: ${ColorfulConsoleHelpers.ParseBool(whiteCheck)}");
            ConsoleManager.WriteLine($"$cBlack king checked: ${ColorfulConsoleHelpers.ParseBool(blackCheck)}");
        }

        private void DisplayCastlingFlags(Command command)
        {
            var castlingFlags = VisualBoard.FriendlyBoard.Castling;

            ConsoleManager.WriteLine($"$cWhite short possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteShortCastlingPossibility)}");
            ConsoleManager.WriteLine($"$cWhite long possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteLongCastlingPossibility)}");
            ConsoleManager.WriteLine($"$cBlack short possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackShortCastlingPossibility)}");
            ConsoleManager.WriteLine($"$cBlack long possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackLongCastlingPossibility)}");

            ConsoleManager.WriteLine($"$cWhite done: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteCastlingDone)}");
            ConsoleManager.WriteLine($"$cBlack done: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackCastlingDone)}");
        }

        private void DisplayEvaluation(Command command)
        {
            var evaluation = BitBoard.GetDetailedEvaluation();

            ConsoleManager.WriteLine($"$c\t\tWhite\tBlack");
            ConsoleManager.WriteLine($"$cMaterial:\t$w{evaluation.Material.WhiteMaterial}\t{evaluation.Material.BlackMaterial}");
            ConsoleManager.WriteLine($"$cMobility:\t$w{evaluation.Mobility.WhiteMobility}\t{evaluation.Mobility.BlackMobility}");
            ConsoleManager.WriteLine($"$cCastling:\t$w{evaluation.Castling.WhiteCastling}\t{evaluation.Castling.BlackCastling}");
            ConsoleManager.WriteLine($"$cPosition:\t$w{evaluation.Position.WhitePosition}\t{evaluation.Position.BlackPosition}");
            ConsoleManager.WriteLine($"$cDoubled pawns:\t$w{evaluation.PawnStructure.WhiteDoubledPawns}\t{evaluation.PawnStructure.BlackDoubledPawns}");
            ConsoleManager.WriteLine($"$cIsolated pawns:\t$w{evaluation.PawnStructure.WhiteIsolatedPawns}\t{evaluation.PawnStructure.BlackIsolatedPawns}");
            ConsoleManager.WriteLine($"$cPawn chain:\t$w{evaluation.PawnStructure.WhitePawnChain}\t{evaluation.PawnStructure.BlackPawnChain}");
            ConsoleManager.WriteLine($"$cKing neighbrs:\t$w{evaluation.KingSafety.WhiteAttackedNeighbours}\t{evaluation.KingSafety.BlackAttackedNeighbours}");

            ConsoleManager.WriteLine();
            ConsoleManager.WriteLine($"$cTotal: $w{evaluation.Total}");
        }

        private void DisplayBoardHash(Command command)
        {
            var trueHash = BitBoard.Hash.ToString();
            ConsoleManager.WriteLine($"$c{trueHash}");
        }
    }
}
