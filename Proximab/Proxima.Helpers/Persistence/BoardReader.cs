using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Notation;
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
            FriendlyPiecesList pieces = null;
            FriendlyCastling castling = null;
            FriendlyEnPassant enPassant = null;

            using (var reader = new StreamReader(path))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                        continue;

                    switch(line)
                    {
                        case "!Board": { pieces = ReadBoard(reader); break; }
                        case "!Castling": { castling = ReadCastling(reader); break; }
                        case "!EnPassant": { enPassant = ReadEnPassant(reader); break; }
                    }  
                }
            }

            return new FriendlyBoard(pieces, castling, enPassant);
        }

        FriendlyPiecesList ReadBoard(StreamReader reader)
        {
            var pieces = new FriendlyPiecesList();

            for (int y = 0; y < 8; y++)
            {
                var line = reader.ReadLine().Trim();
                var splittedLine = line.Split(' ');

                for (int x = 0; x < 8; x++)
                {
                    if (splittedLine[x] == "00")
                        continue;

                    var position = new Position(x + 1, 8 - y);
                    var color = ColorConverter.GetColor(splittedLine[x][0]);
                    var piece = PieceConverter.GetPiece(splittedLine[x][1]);

                    pieces.Add(new FriendlyPiece(position, piece, color));
                }
            }

            return pieces;
        }

        FriendlyCastling ReadCastling(StreamReader reader)
        {
            var castling = new FriendlyCastling();

            castling.WhiteShortCastling = Boolean.Parse(reader.ReadLine().Trim());
            castling.WhiteLongCastling = Boolean.Parse(reader.ReadLine().Trim());
            castling.BlackShortCastling = Boolean.Parse(reader.ReadLine().Trim());
            castling.BlackLongCastling = Boolean.Parse(reader.ReadLine().Trim());

            return castling;
        }

        FriendlyEnPassant ReadEnPassant(StreamReader reader)
        {
            var enPassant = new FriendlyEnPassant();

            enPassant.WhiteEnPassant = ReadPosition(reader);
            enPassant.BlackEnPassant = ReadPosition(reader);

            return enPassant;
        }

        Position ReadPosition(StreamReader reader)
        {
            var line = reader.ReadLine().Trim();

            if (line == "NULL")
                return null;

            var x = Int32.Parse(line[0].ToString());
            var y = Int32.Parse(line[1].ToString());

            return new Position(x, y);
        }
    }
}
