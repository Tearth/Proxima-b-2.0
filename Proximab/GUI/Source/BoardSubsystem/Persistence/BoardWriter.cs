using Core.Board;
using Core.Common;
using System.IO;

namespace GUI.Source.BoardSubsystem.Persistence
{
    internal class BoardWriter
    {
        public BoardWriter()
        {

        }

        public void Write(string path, FriendlyBoard friendlyBoard)
        {
            using (var writer = new StreamWriter(path))
            {
                for(int y = 0; y < 8; y++)
                {
                    for(int x = 0; x < 8; x++)
                    {
                        var position = new Position(x + 1, 8 - y);

                        var field = friendlyBoard.GetPiece(position);
                        writer.Write((int)field);
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
