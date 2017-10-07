using Core.Boards;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Positions;
using System;
using System.IO;

namespace GUI.Source.BoardSubsystem.Persistence
{
    internal class BoardReader
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
                for(int y = 0; y < 8; y++)
                {
                    var line = reader.ReadLine();
                    var splittedLine = line.Split(' ');

                    for(int x = 0; x < 8; x++)
                    {
                        if (splittedLine[x] == "0")
                            continue;

                        var color = (Color)Int32.Parse(splittedLine[x][0].ToString());
                        var piece = (PieceType)Int32.Parse(splittedLine[x][1].ToString());

                        var position = new Position(x + 1, 8 - y);
                        friendlyBoard.SetPiece(position, new FriendlyPiece(piece, color));
                    }
                }
            }

            return friendlyBoard;
        }
    }
}
