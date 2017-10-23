using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
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
                writer.WriteLine("!Board");
                for(int y = 0; y < 8; y++)
                {
                    for(int x = 0; x < 8; x++)
                    {
                        var field = friendlyBoard.Pieces[x, 7 - y];

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

                writer.WriteLine();
                writer.WriteLine("!Castling");

                for(int i=0; i<4; i++)
                {
                    writer.WriteLine(friendlyBoard.Castling[i]);
                }
            }
        }
    }
}
