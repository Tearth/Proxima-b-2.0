using System;
using System.Collections.Generic;
using System.Linq;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.OpeningBook;
using Proxima.Core.Session.Exceptions;
using Proxima.Core.Time;

namespace Proxima.Core.Session
{
    /// <summary>
    /// Represents a set of methods to manage game session (it's recommended to use this class instead
    /// manual creating <see cref="Proxima.Core.Boards.Bitboard"/> class.
    /// </summary>
    public class GameSession
    {
        /// <summary>
        /// Gets the bitboard.
        /// </summary>
        public Bitboard Bitboard { get; private set; }

        /// <summary>
        /// Gets the moves count (where 1 move = white move + black move).
        /// </summary>
        public int MovesCount { get; private set; }

        /// <summary>
        /// The event triggered when there is new thinking output available.
        /// </summary>
        public event EventHandler<ThinkingOutputEventArgs> OnThinkingOutput;

        /// <summary>
        /// The event triggered when there game has ended.
        /// </summary>
        public event EventHandler<GameEndedEventArgs> OnGameEnded;

        public int WhiteRemainingTime => _remainingTime[(int)Color.White];

        public int BlackRemainingTime => _remainingTime[(int)Color.Black];

        private AICore _aiCore;
        private PreferredTimeCalculator _preferredTimeCalculator;

        private int[] _remainingTime;
        private List<Move> _history;

        private OpeningBookProvider _openingBook;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSession"/> class.
        /// </summary>
        public GameSession()
        {
            MovesCount = 0;

            _aiCore = new AICore();
            _aiCore.OnThinkingOutput += AICore_OnThinkingOutput;

            Bitboard = new Bitboard(new DefaultFriendlyBoard());
            _preferredTimeCalculator = new PreferredTimeCalculator(50);

            _remainingTime = new int[2]
            {
                999999999,
                999999999
            };

            _history = new List<Move>();

            _openingBook = new OpeningBookProvider();
        }

        /// <summary>
        /// Moves a piece with the specified color and source/target positions.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The target piece position.</param>
        public void Move(Color color, Position from, Position to)
        {
            Bitboard.Calculate(GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks, false);
            CheckBitboardIntegrity();

            UpdateMovesCount(color);
            CheckIfGameHasEnded();
            if (CheckIfGameHasEnded())
            {
                return;
            }

            // Temporary but I think that's not necessary
            var moveToApply = Bitboard.Moves.FirstOrDefault(p => p.From == from && p.To == to);
            if (moveToApply == null)
            {
                Console.WriteLine($"{from} {to} not found");
                return;
            }

            Bitboard = Bitboard.Move(moveToApply);
            _history.Add(moveToApply);
        }

        /// <summary>
        /// Moves a piece with the specified color and source/target positions and promotes to the new
        /// piece with the specified type.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The target piece position.</param>
        /// <param name="promotionPieceType">The promotion piece type.</param>
        public void Move(Color color, Position from, Position to, PieceType promotionPieceType)
        {
            Bitboard.Calculate(GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks, false);
            CheckBitboardIntegrity();

            UpdateMovesCount(color);
            if (CheckIfGameHasEnded())
            {
                return;
            }

            var possibleMovesToApply = Bitboard.Moves
                .OfType<PromotionMove>()
                .FirstOrDefault(p => p.From == from &&
                            p.To == to &&
                            p.PromotionPiece == promotionPieceType);

            // Temporary but I think that's not necessary
            if (possibleMovesToApply == null)
            {
                Console.WriteLine($"{from} {to} not found");
                return;
            }

            Bitboard = Bitboard.Move(possibleMovesToApply);
            _history.Add(possibleMovesToApply);
        }

        /// <summary>
        /// Runs AI and does best found move.
        /// </summary>
        /// <param name="color">The engine color.</param>
        /// <returns>The AI result.</returns>
        public AIResult MoveAI(Color color)
        {
            Bitboard.Calculate(GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks, false);
            CheckBitboardIntegrity();

            UpdateMovesCount(color);
            if (CheckIfGameHasEnded())
            {
                return null;
            }

            var openingBookMove = _openingBook.GetMoveFromBook(_history);

            if (openingBookMove != null)
            {
                var moveToApply = Bitboard.Moves.First(p => p.From == openingBookMove.From && p.To == openingBookMove.To);

                Bitboard = Bitboard.Move(moveToApply);
                _history.Add(moveToApply);

                return new AIResult
                {
                    PVNodes = new PVNodesList { moveToApply }
                };
            }

            var preferredTime = _preferredTimeCalculator.Calculate(MovesCount, _remainingTime[(int)color]);
            var aiResult = _aiCore.Calculate(color, Bitboard, preferredTime);

            Bitboard = Bitboard.Move(aiResult.PVNodes[0]);
            _history.Add(aiResult.PVNodes[0]);

            return aiResult;
        }

        /// <summary>
        /// Updates remaining time for the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="remainingTime">The remaining time (in seconds).</param>
        public void UpdateRemainingTime(Color color, int remainingTime)
        {
            _remainingTime[(int)color] = remainingTime;
        }

        /// <summary>
        /// The event handler tor OnThinkingOutput.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AICore_OnThinkingOutput(object sender, ThinkingOutputEventArgs e)
        {
            OnThinkingOutput?.Invoke(this, e);
        }

        /// <summary>
        /// Updates moves count for the specified color (increments when current color is white).
        /// </summary>
        /// <param name="color">The current color.</param>
        private void UpdateMovesCount(Color color)
        {
            if (color == Color.White)
            {
                MovesCount++;
            }
        }

        /// <summary>
        /// Checks if bitboard is integrated (throws exception if result is not true).
        /// </summary>
        /// <exception cref="BitboardDisintegratedException">Thrown when bitboard is not integrated.</exception>
        private void CheckBitboardIntegrity()
        {
            if (!Bitboard.VerifyIntegrity())
            {
                throw new BitboardDisintegratedException();
            }
        }

        /// <summary>
        /// Checks if game has ended. If true, OnGameEnded event is invoked.
        /// </summary>
        private bool CheckIfGameHasEnded()
        {
            GameResult? mateResult = null;

            if (Bitboard.IsMate(Color.White))
            {
                mateResult = GameResult.BlackWon;
            }
            else if (Bitboard.IsMate(Color.Black))
            {
                mateResult = GameResult.WhiteWon;
            }
            else if (Bitboard.IsStalemate(Color.White) || Bitboard.IsStalemate(Color.Black) ||
                     Bitboard.IsThreefoldRepetition())
            {
                mateResult = GameResult.Draw;
            }

            if (mateResult.HasValue)
            {
                OnGameEnded?.Invoke(this, new GameEndedEventArgs(mateResult.Value));
                return true;
            }

            return false;
        }
    }
}
