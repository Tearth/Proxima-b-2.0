using Core.Board;
using Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                    for(int x = 0; x < 8; x++)
                    {
                        var pieceChar = line[x].ToString();
                        var pieceType = (PieceType)Enum.Parse(typeof(PieceType), pieceChar);

                        var position = new Position(x + 1, 8 - y);
                        friendlyBoard.SetPiece(position, pieceType);
                    }
                }
            }

            return friendlyBoard;
        }
    }
}
