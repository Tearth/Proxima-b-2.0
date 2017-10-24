using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System;
using System.IO;

namespace Proxima.Helpers.BoardSubsystem.Persistence
{
    public class BoardReader
    {
        public BoardReader()
        {

        }

        public bool BoardExists(string path)
        {
            return File.Exists(path);
        }

        public FriendlyBoard Read(string path)
        {
            var friendlyBoard = new FriendlyBoard();

            using (var reader = new StreamReader(path))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                        continue;

                    switch(line)
                    {
                        case "!Board": { friendlyBoard.Pieces = ReadBoard(reader); break; }
                        case "!Castling": { friendlyBoard.Castling = ReadCastling(reader); break; }
                        case "!WhiteEnPassant": { friendlyBoard.EnPassant[(int)Color.White] = ReadEnPassant(reader); break; }
                        case "!BlackEnPassant": { friendlyBoard.EnPassant[(int)Color.Black] = ReadEnPassant(reader); break; }
                    }  
                }
            }

            return friendlyBoard;
        }

        FriendlyPiece[,] ReadBoard(StreamReader reader)
        {
            var pieces = new FriendlyPiece[8, 8];

            for (int y = 0; y < 8; y++)
            {
                var line = reader.ReadLine().Trim();
                var splittedLine = line.Split(' ');

                for (int x = 0; x < 8; x++)
                {
                    if (splittedLine[x] == "0")
                        continue;

                    var color = (Color)Int32.Parse(splittedLine[x][0].ToString());
                    var piece = (PieceType)Int32.Parse(splittedLine[x][1].ToString());
                    var friendlyPiece = new FriendlyPiece(piece, color);
                    
                    pieces[x, y] = friendlyPiece;
                }
            }

            return pieces;
        }

        bool[,] ReadCastling(StreamReader reader)
        {
            var castling = new bool[2, 2];

            for(int i=0; i<4; i++)
            {
                var line = reader.ReadLine().Trim();
                castling[(i % 2), (i / 2)] = Boolean.Parse(line);
            }

            return castling;
        }      

        bool[,] ReadEnPassant(StreamReader reader)
        {
            bool[,] enPassant = new bool[8, 8];

            for (int y = 0; y < 8; y++)
            {
                var line = reader.ReadLine().Trim();
                var splittedLine = line.Split(' ');

                for (int x = 0; x < 8; x++)
                { 
                    enPassant[x, y] = splittedLine[x] == "1";
                }
            }

            return enPassant;
        }
    }
}
