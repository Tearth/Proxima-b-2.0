﻿using Proxima.Core.Boards.MoveGenerators;
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

        public void SetDefault()
        {
            //White pieces
            SetPiece(new Position(1, 1), new FriendlyPiece(PieceType.Rook, Color.White));
            SetPiece(new Position(2, 1), new FriendlyPiece(PieceType.Knight, Color.White));
            SetPiece(new Position(3, 1), new FriendlyPiece(PieceType.Bishop, Color.White));
            SetPiece(new Position(4, 1), new FriendlyPiece(PieceType.Queen, Color.White));
            SetPiece(new Position(5, 1), new FriendlyPiece(PieceType.King, Color.White));
            SetPiece(new Position(6, 1), new FriendlyPiece(PieceType.Bishop, Color.White));
            SetPiece(new Position(7, 1), new FriendlyPiece(PieceType.Knight, Color.White));
            SetPiece(new Position(8, 1), new FriendlyPiece(PieceType.Rook, Color.White));

            SetPiece(new Position(1, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(2, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(3, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(4, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(5, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(6, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(7, 2), new FriendlyPiece(PieceType.Pawn, Color.White));
            SetPiece(new Position(8, 2), new FriendlyPiece(PieceType.Pawn, Color.White));

            //Black pieces
            SetPiece(new Position(1, 8), new FriendlyPiece(PieceType.Rook, Color.Black));
            SetPiece(new Position(2, 8), new FriendlyPiece(PieceType.Knight, Color.Black));
            SetPiece(new Position(3, 8), new FriendlyPiece(PieceType.Bishop, Color.Black));
            SetPiece(new Position(4, 8), new FriendlyPiece(PieceType.Queen, Color.Black));
            SetPiece(new Position(5, 8), new FriendlyPiece(PieceType.King, Color.Black));
            SetPiece(new Position(6, 8), new FriendlyPiece(PieceType.Bishop, Color.Black));
            SetPiece(new Position(7, 8), new FriendlyPiece(PieceType.Knight, Color.Black));
            SetPiece(new Position(8, 8), new FriendlyPiece(PieceType.Rook, Color.Black));

            SetPiece(new Position(1, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(2, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(3, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(4, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(5, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(6, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(7, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));
            SetPiece(new Position(8, 7), new FriendlyPiece(PieceType.Pawn, Color.Black));

            //Castling
            Castling[(int)Color.White, (int)CastlingType.Short] = true;
            Castling[(int)Color.White, (int)CastlingType.Long] = true;
            Castling[(int)Color.Black, (int)CastlingType.Short] = true;
            Castling[(int)Color.Black, (int)CastlingType.Long] = true;

            //EnPassant
            EnPassant[(int)Color.White] = new bool[8, 8];
            EnPassant[(int)Color.Black] = new bool[8, 8];
        }
    }
}
