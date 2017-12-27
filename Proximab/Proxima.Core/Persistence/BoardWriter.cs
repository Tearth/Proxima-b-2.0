using System.IO;
using System.Linq;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Persistence
{
    /// <summary>
    /// Represents a set of methods to saving <see cref="FriendlyBoard"/> object as file.
    /// </summary>
    public class BoardWriter
    {
        /// <summary>
        /// Writes <see cref="FriendlyBoard"/> object to the specified file.
        /// </summary>
        /// <param name="path">The path to file.</param>
        /// <param name="friendlyBoard">The board to save.</param>
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

        /// <summary>
        /// Writes a board (pure piece positions).
        /// </summary>
        /// <param name="writer">The file writer.</param>
        /// <param name="pieces">The list of pieces to write.</param>
        private void WriteBoard(StreamWriter writer, FriendlyPiecesList pieces)
        {
            writer.WriteLine(PersistenceConstants.BoardSection);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = pieces.FirstOrDefault(p => p.Position == new Position(x + 1, 8 - y));

                    if (field == null)
                    {
                        writer.Write(PersistenceConstants.EmptyBoardField);
                    }
                    else
                    {
                        writer.Write(ColorConverter.GetSymbol(field.Color));
                        writer.Write(PieceConverter.GetSymbol(field.Type));
                    }

                    writer.Write(" ");
                }

                writer.WriteLine();
            }
        }

        /// <summary>
        /// Writes a castling flags.
        /// </summary>
        /// <param name="writer">The file writer.</param>
        /// <param name="castling">The castling data.</param>
        private void WriteCastling(StreamWriter writer, FriendlyCastling castling)
        {
            writer.WriteLine(PersistenceConstants.CastlingSection);

            writer.WriteLine(castling.WhiteShortCastlingPossibility.ToString());
            writer.WriteLine(castling.WhiteLongCastlingPossibility.ToString());
            writer.WriteLine(castling.BlackShortCastlingPossibility.ToString());
            writer.WriteLine(castling.BlackLongCastlingPossibility.ToString());

            writer.WriteLine(castling.WhiteCastlingDone.ToString());
            writer.WriteLine(castling.BlackCastlingDone.ToString());
        }

        /// <summary>
        /// Writes en passant data. 
        /// </summary>
        /// <param name="writer">The file writer.</param>
        /// <param name="enPassant">The en passant data.</param>
        private void WriteEnPassant(StreamWriter writer, FriendlyEnPassant enPassant)
        {
            writer.WriteLine(PersistenceConstants.EnPassantSection);

            WritePosition(writer, enPassant.WhiteEnPassant);
            WritePosition(writer, enPassant.BlackEnPassant);
        }

        /// <summary>
        /// Writes a <see cref="Position"/> object to the file. 
        /// </summary>
        /// <param name="writer">The file writer.</param>
        /// <param name="position">The position to write (writes <see cref="PersistenceConstants.NullValue"/> if null).</param>
        private void WritePosition(StreamWriter writer, Position? position)
        {
            if (!position.HasValue)
            {
                writer.WriteLine(PersistenceConstants.NullValue);
                return;
            }

            writer.WriteLine(PositionConverter.ToString(position.Value));
        }
    }
}
