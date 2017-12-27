using System.IO;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Persistence
{
    /// <summary>
    /// Represents a set of methods to loading <see cref="FriendlyBoard"/> object from file.
    /// </summary>
    public class BoardReader
    {
        /// <summary>
        /// Checks if the specified file exists.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>True if the file exists, otherwise false.</returns>
        public bool BoardExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Loads <see cref="FriendlyBoard"/> object from the specified file.
        /// </summary>
        /// <param name="path">The path to file.</param>
        /// <returns><see cref="FriendlyBoard"/> object loaded from the file.</returns>
        public FriendlyBoard Read(string path)
        {
            FriendlyPiecesList pieces = null;
            FriendlyCastling castling = null;
            FriendlyEnPassant enPassant = null;

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    switch (line)
                    {
                        case PersistenceConstants.BoardSection:
                        {
                            pieces = ReadBoard(reader);
                            break;
                        }

                        case PersistenceConstants.CastlingSection:
                        {
                            castling = ReadCastling(reader);
                            break;
                        }

                        case PersistenceConstants.EnPassantSection:
                        {
                            enPassant = ReadEnPassant(reader);
                            break;
                        }
                    }
                }
            }

            return new FriendlyBoard(pieces, castling, enPassant);
        }

        /// <summary>
        /// Reads a board (pure piece positions).
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <returns>The container object with the list of pieces.</returns>
        private FriendlyPiecesList ReadBoard(StreamReader reader)
        {
            var pieces = new FriendlyPiecesList();

            for (int y = 0; y < 8; y++)
            {
                var line = reader.ReadLine().Trim();
                var splitLine = line.Split(' ');

                for (int x = 0; x < 8; x++)
                {
                    if (splitLine[x] == PersistenceConstants.EmptyBoardField)
                    {
                        continue;
                    }

                    var position = new Position(x + 1, 8 - y);
                    var color = ColorConverter.GetColor(splitLine[x][0]);
                    var piece = PieceConverter.GetPiece(splitLine[x][1]);

                    pieces.Add(new FriendlyPiece(position, piece, color));
                }
            }

            return pieces;
        }

        /// <summary>
        /// Reads a castling flags.
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <returns>The container object with the castling flags.</returns>
        private FriendlyCastling ReadCastling(StreamReader reader)
        {
            return new FriendlyCastling
            {
                WhiteShortCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                WhiteLongCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                BlackShortCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                BlackLongCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),

                WhiteCastlingDone = bool.Parse(reader.ReadLine().Trim()),
                BlackCastlingDone = bool.Parse(reader.ReadLine().Trim())
            };
        }

        /// <summary>
        /// Reads en passant data. 
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <returns>The container object with the en passant data.</returns>
        private FriendlyEnPassant ReadEnPassant(StreamReader reader)
        {
            return new FriendlyEnPassant
            {
                WhiteEnPassant = ReadPosition(reader),
                BlackEnPassant = ReadPosition(reader)
            };
        }

        /// <summary>
        /// Reads a <see cref="Position"/> object to the file. 
        /// </summary>
        /// <param name="reader">The file writer.</param>
        /// <returns>The read position (or null if there was a <see cref="PersistenceConstants.NullValue"/> in the file).</returns>
        private Position? ReadPosition(StreamReader reader)
        {
            var line = reader.ReadLine().Trim();

            if (line == PersistenceConstants.NullValue)
            {
                return null;
            }

            return PositionConverter.ToPosition(line);
        }
    }
}
