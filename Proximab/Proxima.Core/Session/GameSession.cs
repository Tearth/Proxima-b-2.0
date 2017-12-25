using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.AI;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Exceptions;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;
using Proxima.Core.MoveGenerators.Moves;
using Proxima.Core.Time;

namespace Proxima.Core.Session
{
    public class GameSession
    {
        public Bitboard Bitboard { get; private set; }
        public int MovesCount { get; private set; }

        public event EventHandler<ThinkingOutputEventArgs> OnThinkingOutput;

        private AICore _aiCore;
        private PreferredTimeCalculator _preferredTimeCalculator;

        private int[] _remainingTime;

        public GameSession()
        {
            MovesCount = 0;

            _aiCore = new AICore();
            _aiCore.OnThinkingOutput += AICore_OnThinkingOutput;

            Bitboard = new Bitboard(new DefaultFriendlyBoard());
            _preferredTimeCalculator = new PreferredTimeCalculator(60);

            _remainingTime = new int[2];
        }

        public void Move(Color color, Position from, Position to)
        {
            UpdateMovesCount(color);

            Bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves, false);

            var moveToApply = Bitboard.Moves.First(p => p.From == from && p.To == to);

            Bitboard = Bitboard.Move(moveToApply);
            CheckBitboardIntegrity();
        }

        public void Move(Color color, Position from, Position to, PieceType promotionPieceType)
        {
            UpdateMovesCount(color);

            Bitboard.Calculate(GeneratorMode.CalculateMoves, GeneratorMode.CalculateMoves, false);

            var possibleMovesToApply = Bitboard.Moves.Cast<PromotionMove>()
                .First(p => p.From == from && p.To == to &&
                       p.PromotionPiece == promotionPieceType);

            Bitboard = Bitboard.Move(possibleMovesToApply);
            CheckBitboardIntegrity();
        }

        public AIResult MoveAI(Color color)
        {
            UpdateMovesCount(color);

            var preferredTime = _preferredTimeCalculator.Calculate(MovesCount, _remainingTime[(int)color]);
            var aiResult = _aiCore.Calculate(color, Bitboard, preferredTime);

            Bitboard = Bitboard.Move(aiResult.BestMove);
            CheckBitboardIntegrity();

            return aiResult;
        }

        public void UpdateRemainingTime(Color color, int remainingTime)
        {
            _remainingTime[(int)color] = remainingTime;
        }

        private void AICore_OnThinkingOutput(object sender, ThinkingOutputEventArgs e)
        {
            OnThinkingOutput?.Invoke(this, e);
        }

        private void UpdateMovesCount(Color color)
        {
            if(color == Color.White)
            {
                MovesCount++;
            }
        }

        private void CheckBitboardIntegrity()
        {
            if (!Bitboard.VerifyIntegrity())
            {
                throw new BitboardDisintegratedException();
            }
        }
    }
}
