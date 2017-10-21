using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;
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
                for(int y = 0; y < 8; y++)
                {
                    for(int x = 0; x < 8; x++)
                    {
                        var position = new Position(x + 1, 8 - y);

                        var field = friendlyBoard.GetPiece(position);

                        if(field == null)
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
        }
    }
}
