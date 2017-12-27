using System;
using System.Collections.Generic;
using GUI.App.BoardSubsystem;
using GUI.App.BoardSubsystem.Pieces;
using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using GUI.App.InputSubsystem;
using GUI.App.PromotionSubsystem;
using Helpers.ColorfulConsole;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Persistence;

namespace GUI.App.GameSubsystem
{
    /// <summary>
    /// Represents a set of methods common for all game modes (logic, drawing, commands).
    /// </summary>
    public abstract class GameModeBase
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
        protected Bitboard Bitboard { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModeBase"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager instance</param>
        /// <param name="commandsManager">The commands manager instance</param>
        protected GameModeBase(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            ConsoleManager = consoleManager;
            CommandsManager = commandsManager;

            PiecesProvider = new PiecesProvider();

            VisualBoard = new VisualBoard(PiecesProvider);
            PromotionWindow = new PromotionWindow(PiecesProvider);

            SetCommandHandlers();
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
        /// Applies move to the bitboard and updates the visual board (generator mode is set to CalculateAttacks for both colors).
        /// </summary>
        /// <param name="move">The move to apply.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        protected void CalculateBitboard(Move move, bool quiescenceSearch)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitboard(move, mode, mode, quiescenceSearch);
        }

        /// <summary>
        /// Applies move to the bitboard and updates the visual board.
        /// </summary>
        /// <param name="move">The move to apply.</param>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        protected void CalculateBitboard(Move move, GeneratorMode whiteMode, GeneratorMode blackMode, bool quiescenceSearch)
        {
            Bitboard = Bitboard.Move(move);
            Bitboard.Calculate(whiteMode, blackMode, quiescenceSearch);

            VisualBoard.FriendlyBoard = new FriendlyBoard(Bitboard);
        }

        /// <summary>
        /// Applies friendly board to the bitboard and updates the visual board (generator mode is set to CalculateAttacks for both colors).
        /// </summary>
        /// <param name="friendlyBoard">The friendly board to apply.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        protected void CalculateBitboard(FriendlyBoard friendlyBoard, bool quiescenceSearch)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitboard(friendlyBoard, mode, mode, quiescenceSearch);
        }

        /// <summary>
        /// Applies friendly board to the bitboard and updates the visual board.
        /// </summary>
        /// <param name="friendlyBoard">The friendly board to apply.</param>
        /// <param name="whiteMode">The white generator mode.</param>
        /// <param name="blackMode">The black generator mode.</param>
        /// <param name="quiescenceSearch">If true, only quiescence moves (mainly captures) will be generated.</param>
        protected void CalculateBitboard(FriendlyBoard friendlyBoard, GeneratorMode whiteMode, GeneratorMode blackMode, bool quiescenceSearch)
        {
            Bitboard = new Bitboard(friendlyBoard);
            Bitboard.Calculate(whiteMode, blackMode, quiescenceSearch);

            VisualBoard.FriendlyBoard = new FriendlyBoard(Bitboard);
        }

        /// <summary>
        /// Adds all command handlers from current class to the commands manager.
        /// </summary>
        private void SetCommandHandlers()
        {
            CommandsManager.AddCommandHandler(CommandType.Occupancy, CommandGroup.GameMode, DrawOccupancy);
            CommandsManager.AddCommandHandler(CommandType.Attacks, CommandGroup.GameMode, DrawAttacks);
            CommandsManager.AddCommandHandler(CommandType.SaveBoard, CommandGroup.GameMode, SaveBoard);
            CommandsManager.AddCommandHandler(CommandType.LoadBoard, CommandGroup.GameMode, LoadBoard);
            CommandsManager.AddCommandHandler(CommandType.Check, CommandGroup.GameMode, DisplayCheckStatus);
            CommandsManager.AddCommandHandler(CommandType.Mate, CommandGroup.GameMode, DisplayMateStatus);
            CommandsManager.AddCommandHandler(CommandType.Stalemate, CommandGroup.GameMode, DisplayStalemateStatus);
            CommandsManager.AddCommandHandler(CommandType.Castling, CommandGroup.GameMode, DisplayCastlingFlags);
            CommandsManager.AddCommandHandler(CommandType.Evaluation, CommandGroup.GameMode, DisplayEvaluation);
            CommandsManager.AddCommandHandler(CommandType.Hash, CommandGroup.GameMode, DisplayBoardHash);
            CommandsManager.AddCommandHandler(CommandType.Reset, CommandGroup.GameMode, Reset);
        }

        /// <summary>
        /// Draws occupancy (nonempty fields) by pieces with the specified color.
        /// </summary>
        /// <param name="command">The Occupancy command.</param>
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
        /// <param name="command">The Attacks command.</param>
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
        /// <param name="command">The Save command.</param>
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
        /// <param name="command">The Load command.</param>
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

            CalculateBitboard(boardReader.Read(path), false);
        }

        /// <summary>
        /// Displays check status on the console.
        /// </summary>
        /// <param name="command">The Check command.</param>
        private void DisplayCheckStatus(Command command)
        {
            var whiteCheck = Bitboard.IsCheck(Color.White);
            var blackCheck = Bitboard.IsCheck(Color.Black);

            ConsoleManager.WriteLine($"$cWhite king checked: ${ColorfulConsoleHelpers.ParseBool(whiteCheck)}");
            ConsoleManager.WriteLine($"$cBlack king checked: ${ColorfulConsoleHelpers.ParseBool(blackCheck)}");
        }

        /// <summary>
        /// Displays mate status on the console.
        /// </summary>
        /// <param name="command">The Mate command.</param>
        private void DisplayMateStatus(Command command)
        {
            var whiteMate = Bitboard.IsMate(Color.White);
            var blackMate = Bitboard.IsMate(Color.Black);

            ConsoleManager.WriteLine($"$cWhite king mated: ${ColorfulConsoleHelpers.ParseBool(whiteMate)}");
            ConsoleManager.WriteLine($"$cBlack king mated: ${ColorfulConsoleHelpers.ParseBool(blackMate)}");
        }

        /// <summary>
        /// Displays stalemate status on the console.
        /// </summary>
        /// <param name="command">The Stalemate command.</param>
        private void DisplayStalemateStatus(Command command)
        {
            var whiteStalemate = Bitboard.IsStalemate(Color.White);
            var blackStalemate = Bitboard.IsStalemate(Color.Black);

            ConsoleManager.WriteLine($"$cWhite king in stalemate: ${ColorfulConsoleHelpers.ParseBool(whiteStalemate)}");
            ConsoleManager.WriteLine($"$cBlack king in stalemate: ${ColorfulConsoleHelpers.ParseBool(blackStalemate)}");
        }

        /// <summary>
        /// Displays castling flags on the console.
        /// </summary>
        /// <param name="command">The Castling command.</param>
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
        /// Displays board hash on the console.
        /// </summary>
        /// <param name="command">The Hash command.</param>
        private void DisplayBoardHash(Command command)
        {
            var trueHash = Bitboard.Hash.ToString();
            ConsoleManager.WriteLine($"$c{trueHash}");
        }

        /// <summary>
        /// Displays evaluation result on the console.
        /// </summary>
        /// <param name="command">The Evaluation command.</param>
        private void DisplayEvaluation(Command command)
        {
            var evaluation = Bitboard.GetDetailedEvaluation();

            ConsoleManager.WriteLine("$c\t\tWhite\tBlack");
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
        /// Resets board to the default state.
        /// </summary>
        /// <param name="command">The Reset command.</param>
        private void Reset(Command command)
        {
            CalculateBitboard(new DefaultFriendlyBoard(), false);
        }
    }
}