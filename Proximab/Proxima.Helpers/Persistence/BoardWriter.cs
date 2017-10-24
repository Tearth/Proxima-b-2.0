using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System;
using System.IO;

namespace Proxima.Helpers.BoardSubsystem.Persistence
{
    public class BoardWriter
    {
        public BoardWriter()
        {

        }

        public void Write(string path, FriendlyBoard friendlyBoard)
        {
            using (var writer = new StreamWriter(path))
            {
                WriteBoard(writer, friendlyBoard.Pieces);
                writer.WriteLine();
                WriteCastling(writer, friendlyBoard.Castling);
                writer.WriteLine();
                WriteEnPassant(writer, friendlyBoard.EnPassant);
            }
        }

        void WriteBoard(StreamWriter writer, FriendlyPiece[,] pieces)
        {
            writer.WriteLine("!Board");
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = pieces[x, y];

                    if (field == null)
                    {
                        writer.Write('0');
                    }
                    else
                    {
                        writer.Write((int)field.Color);
                        writer.Write((int)field.Type);
                    }

                    writer.Write(' ');
                }

                writer.WriteLine();
            }
        }

        void WriteCastling(StreamWriter writer, bool[,] castling)
        {
            writer.WriteLine("!Castling");

            for(int color = 0; color < 2; color++)
            {
                for(int castlingType = 0; castlingType < 2; castlingType++)
                {
                    writer.WriteLine(castling[color, castlingType]);
                }
            }
        }

        void WriteEnPassant(StreamWriter writer, bool[][,] enPassant)
        {
            writer.WriteLine("!WhiteEnPassant");
            WriteEnPassant(writer, enPassant[(int)Color.White]);
            writer.WriteLine();
            writer.WriteLine("!BlackEnPassant");
            WriteEnPassant(writer, enPassant[(int)Color.Black]);
        }

        void WriteEnPassant(StreamWriter writer, bool[,] enPassant)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = Convert.ToInt32(enPassant[x, y]);
                    writer.Write(field);
                    writer.Write(' ');
                }

                writer.WriteLine();
            }
        }
    }
}
