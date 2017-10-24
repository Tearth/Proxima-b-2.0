using Proxima.Core.Boards.MoveGenerators;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Positions;
using System;

namespace Proxima.Core.Boards.Friendly
{
    public class FriendlyBoard
    {
        public FriendlyPiece[,] Pieces { get; set; }
        public bool[,] Castling { get; set; }
        public bool[][,] EnPassant { get; set; }

        public FriendlyBoard()
        {
            Pieces = new FriendlyPiece[8, 8];
            Castling = new bool[2,2];
            EnPassant = new bool[2][,];
        }

        public FriendlyBoard(ulong[] piecesArray, bool[] castling, ulong[] enPassant) : this()
        {
            for (int i = 0; i < 12; i++)
            {
                var pieceArray = piecesArray[i];

                while (pieceArray != 0)
                {
                    var lsb = BitOperations.GetLSB(ref pieceArray);
                    var bitIndex = BitOperations.GetBitIndex(lsb);
                    var position = BitPositionConverter.ToPosition(bitIndex);

                    SetPiece(position, new FriendlyPiece((PieceType)(i % 6), (Color)(i / 6)));
                }
            }
            
            Castling[(int)Color.White, (int)CastlingType.Short] = castling[0];
            Castling[(int)Color.White, (int)CastlingType.Long] = castling[1];
            Castling[(int)Color.Black, (int)CastlingType.Short] = castling[2];
            Castling[(int)Color.Black, (int)CastlingType.Long] = castling[3];

            EnPassant[(int)Color.White] = BitPositionConverter.ToBoolArray(enPassant[(int)Color.White]);
            EnPassant[(int)Color.Black] = BitPositionConverter.ToBoolArray(enPassant[(int)Color.Black]);
        }

        public FriendlyPiece GetPiece(Position position)
        {
            return Pieces[position.X - 1, 8 - position.Y];
        }

        public void SetPiece(Position position, FriendlyPiece piece)
        {
            Pieces[position.X - 1, 8 - position.Y] = piece;
        }

        public ulong[] GetPiecesArray()
        {
            var pieces = new ulong[12];

            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    var position = new Position(x, y);
                    var piece = GetPiece(position);

                    if (piece != null)
                    {
                        var bitPosition = BitPositionConverter.ToULong(position);
                        pieces[FastArray.GetPieceIndex(piece.Color, piece.Type)] |= bitPosition;
                    }
                }
            }

            return pieces;
        }

        public bool[] GetCastlingArray()
        {
            var castling = new bool[4];

            castling[0] = Castling[(int)Color.White, (int)CastlingType.Short];
            castling[1] = Castling[(int)Color.White, (int)CastlingType.Long];
            castling[2] = Castling[(int)Color.Black, (int)CastlingType.Short];
            castling[3] = Castling[(int)Color.Black, (int)CastlingType.Long];

            return castling;
        }

        public ulong[] GetEnPassantArray()
        {
            ulong[] enPassant = new ulong[2];

            enPassant[(int)Color.White] = BitPositionConverter.ToULong(EnPassant[(int)Color.White]);
            enPassant[(int)Color.Black] = BitPositionConverter.ToULong(EnPassant[(int)Color.Black]);

            return enPassant;
        }
    }
}
