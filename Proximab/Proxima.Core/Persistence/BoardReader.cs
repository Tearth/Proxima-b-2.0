using System.IO;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;
using Proxima.Core.Persistence.Exceptions;

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
        /// <exception cref="InvalidSectionValueException">Thrown when loaded section name has invalid value.</exception>
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
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        throw new InvalidSectionValueException();
                    }

                    var lineAfterTrim = line.Trim();
                    if (lineAfterTrim.Length == 0)
                    {
                        continue;
                    }

                    switch (lineAfterTrim)
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

                        default:
                        {
                            throw new InvalidSectionValueException();
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
        /// <exception cref="InvalidBoardValueException">Thrown when a loaded board state cannot be converted properly.</exception>
        /// <returns>The container object with the list of pieces.</returns>
        private FriendlyPiecesList ReadBoard(StreamReader reader)
        {
            var pieces = new FriendlyPiecesList();

            for (var y = 0; y < 8; y++)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    throw new InvalidBoardValueException();
                }

                var lineAfterTrim = line.Trim();

                var splitLine = lineAfterTrim.Split(' ');
                if (splitLine.Length != 8)
                {
                    throw new InvalidBoardValueException();
                }

                for (var x = 0; x < 8; x++)
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
                WhiteShortCastlingPossibility = ReadFlag(reader),
                WhiteLongCastlingPossibility = ReadFlag(reader),
                BlackShortCastlingPossibility = ReadFlag(reader),
                BlackLongCastlingPossibility = ReadFlag(reader),

                WhiteCastlingDone = ReadFlag(reader),
                BlackCastlingDone = ReadFlag(reader)
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
        /// Reads a <see cref="Position"/> object.
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <exception cref="InvalidPositionValueException">Thrown when a loaded position cannot be converted properly.</exception>
        /// <returns>The loaded position (or null if there was a <see cref="PersistenceConstants.NullValue"/> in the file).</returns>
        private Position? ReadPosition(StreamReader reader)
        {
            var line = reader.ReadLine();
            if (line == null)
            {
                throw new InvalidPositionValueException();
            }

            var lineAfterTrim = line.Trim();
            if (lineAfterTrim == PersistenceConstants.NullValue)
            {
                return null;
            }

            return PositionConverter.ToPosition(lineAfterTrim);
        }

        /// <summary>
        /// Reads a flag (True/False).
        /// </summary>
        /// <param name="reader">The file reader.</param>
        /// <exception cref="InvalidFlagValueException">Thrown when loaded flag value cannot be converted.</exception>
        /// <returns>The loaded flag.</returns>
        private bool ReadFlag(StreamReader reader)
        {
            var line = reader.ReadLine();
            if (line == null || !bool.TryParse(line.Trim(), out var flag))
            {
                throw new InvalidFlagValueException();
            }

            return flag;
        }
    }
}
