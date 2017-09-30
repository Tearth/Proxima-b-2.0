using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Board
{
    public class FriendlyBoard
    {
        public PieceType[,] Board { get; set; }

        public FriendlyBoard()
        {
            Board = new PieceType[8, 8];

            for(int x=0; x<8; x++)
            {
                for(int y=0; y<8; y++)
                {
                    Board[x, y] = PieceType.None;
                }
            }
        }

        public PieceType GetPieceAtPosition(Position position)
        {
            return Board[position.X - 1, position.Y - 1];
        }
    }
}
