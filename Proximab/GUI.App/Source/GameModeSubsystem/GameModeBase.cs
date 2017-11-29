using System;
using System.Collections.Generic;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.CommandsSubsystem;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.InputSubsystem;
using GUI.App.Source.PromotionSubsystem;
using GUI.ColorfulConsole;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Persistence;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;

namespace GUI.App.Source.GameModeSubsystem
{
    /// <summary>
    /// Represents a set of methods common for all game modes (logic, drawing, commands). 
    /// </summary>
    internal abstract class GameModeBase
    {
        /// <summary>
        /// Gets or sets the console manager.
        /// </summary>
        protected ConsoleManager ConsoleManager { get; set; }

        /// <summary>
        /// Gets or sets the commands manager.
        /// </summary>
        protected CommandsManager CommandsManager { get; set; }

        /// <summary>
        /// Gets or sets the pieces provider.
        /// </summary>
        protected PiecesProvider PiecesProvider { get; set; }

        /// <summary>
        /// Gets or sets the visual board.
        /// </summary>
        protected VisualBoard VisualBoard { get; set; }

        /// <summary>
        /// Gets or sets the promotion window.
        /// </summary>
        protected PromotionWindow PromotionWindow { get; set; }

        /// <summary>
        /// Gets or sets the bitboard.
        /// </summary>
        protected BitBoard BitBoard { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModeBase"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager instance</param>
        /// <param name="commandsManager">The commands manager instance</param>
        public GameModeBase(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            ConsoleManager = consoleManager;
            CommandsManager = commandsManager;

            PiecesProvider = new PiecesProvider();

            VisualBoard = new VisualBoard(PiecesProvider);
            PromotionWindow = new PromotionWindow(PiecesProvider);
        }

        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="contentManager">Monogame content manager.</param>
        public virtual void LoadContent(ContentManager contentManager)
        {
            PiecesProvider.LoadContent(contentManager);
            VisualBoard.LoadContent(contentManager);
            PromotionWindow.LoadContent(contentManager);
        }

        /// <summary>
        /// Processes all events related to mouse and keyboard.
        /// </summary>
        /// <param name="inputManager">InputManager instance.</param>
        public virtual void Input(InputManager inputManager)
        {
            if (!PromotionWindow.Active)
            {
                VisualBoard.Input(inputManager);
            }

            PromotionWindow.Input(inputManager);
        }

        /// <summary>
        /// Processes all logic related to the base game mode.
        /// </summary>
        public virtual void Logic()
        {
            VisualBoard.Logic();
            PromotionWindow.Logic();
        }

        /// <summary>
        /// Draws board and promotion window (optionally).
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            VisualBoard.Draw(spriteBatch);
            PromotionWindow.Draw(spriteBatch);
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        protected virtual void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.Occupancy, DrawOccupancy);
            CommandsManager.AddCommandHandler(CommandType.Attacks, DrawAttacks);
            CommandsManager.AddCommandHandler(CommandType.SaveBoard, SaveBoard);
            CommandsManager.AddCommandHandler(CommandType.LoadBoard, LoadBoard);
            CommandsManager.AddCommandHandler(CommandType.Check, DisplayCheckStatus);
            CommandsManager.AddCommandHandler(CommandType.Castling, DisplayCastlingFlags);
            CommandsManager.AddCommandHandler(CommandType.Evaluation, DisplayEvaluation);
            CommandsManager.AddCommandHandler(CommandType.Hash, DisplayBoardHash);
        }

        /// <summary>
        /// Applies friendly board to the bitboard and updates the visual board (generator mode is set to CalculateAttacks for both colors).
        /// </summary>
        /// <param name="friendlyBoard">The friendly board to apply.</param>
        protected void CalculateBitBoard(FriendlyBoard friendlyBoard)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(friendlyBoard, mode, mode);
        }

        /// <summary>
        /// Applies move to the bitboard and updates the visual board (generator mode is set to CalculateAttacks for both colors).
        /// </summary>
        /// <param name="move">The move to apply.</param>
        protected void CalculateBitBoard(Move move)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(move, mode, mode);
        }

        /// <summary>
        /// Applies friendly board to the bitboard and updates the visual board.
        /// </summary>
        /// <param name="friendlyBoard">The friendly board to apply.</param>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        protected void CalculateBitBoard(FriendlyBoard friendlyBoard, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            BitBoard = new BitBoard(friendlyBoard);
            BitBoard.Calculate(whiteMode, blackMode);

            VisualBoard.FriendlyBoard = new FriendlyBoard(BitBoard);
        }

        /// <summary>
        /// Applies move to the bitboard and updates the visual board.
        /// </summary>
        /// <param name="move">The move to apply.</param>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        protected void CalculateBitBoard(Move move, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            BitBoard = BitBoard.Move(move);
            BitBoard.Calculate(whiteMode, blackMode);

            VisualBoard.FriendlyBoard = new FriendlyBoard(BitBoard);
        }

        /// <summary>
        /// Draws occupancy (nonempty fields) by pieces with the specified color.
        /// </summary>
        /// <param name="command">The passed command.</param>
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

        /// <summary>
        /// Draws all fields attacked by pieces with the specified color.
        /// </summary>
        /// <param name="command">The passed command.</param>
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

        /// <summary>
        /// Saves a board to the specified file.
        /// </summary>
        /// <param name="command">The passed command.</param>
        private void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = VisualBoard.FriendlyBoard;

            var path = $"Boards\\{boardNameArgument}.board";
            boardWriter.Write(path, board);
        }

        /// <summary>
        /// Loads a board from the specified file and updates bitboard.
        /// </summary>
        /// <param name="command">The passed command.</param>
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

        /// <summary>
        /// Displays check status on the console.
        /// </summary>
        /// <param name="command">The passed command.</param>
        private void DisplayCheckStatus(Command command)
        {
            var whiteCheck = BitBoard.IsCheck(Color.White);
            var blackCheck = BitBoard.IsCheck(Color.Black);

            ConsoleManager.WriteLine($"$cWhite king checked: ${ColorfulConsoleHelpers.ParseBool(whiteCheck)}");
            ConsoleManager.WriteLine($"$cBlack king checked: ${ColorfulConsoleHelpers.ParseBool(blackCheck)}");
        }

        /// <summary>
        /// Displays castling flags on the console.
        /// </summary>
        /// <param name="command">The passed command.</param>
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

        /// <summary>
        /// Displays evaluation result on the console.
        /// </summary>
        /// <param name="command">The passed command.</param>
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

        /// <summary>
        /// Displays board hash on the console.
        /// </summary>
        /// <param name="command">The passed command.</param>
        private void DisplayBoardHash(Command command)
        {
            var trueHash = BitBoard.Hash.ToString();
            ConsoleManager.WriteLine($"$c{trueHash}");
        }
    }
}
