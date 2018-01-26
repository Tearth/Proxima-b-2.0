using System.Diagnostics;

namespace Proxima.Core.Commons.Positions
{
    /// <summary>
    /// Represents a board position.
    /// </summary>
    [DebuggerDisplay("[{X} {Y}]")]
    public struct Position
    {
        /// <summary>
        /// Gets or sets the horizontal position.
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        /// Gets or sets the vertical position.
        /// </summary>
        public byte Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> struct.
        /// </summary>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">Vertical coordinate.</param>
        public Position(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> struct.
        /// </summary>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">Vertical coordinate.</param>
        public Position(int x, int y)
        {
            X = (byte)x;
            Y = (byte)y;
        }

        /// <summary>
        /// Checks if the position is valid for chess board (values from 1 to 8).
        /// </summary>
        /// <returns>True if the position is valid, otherwise false.</returns>
        public bool IsValid()
        {
            return X >= 1 && X <= 8 && Y >= 1 && Y <= 8;
        }

        /// <summary>
        /// Compares two position objects.
        /// </summary>
        /// <param name="a">The first position to compare.</param>
        /// <param name="b">The second position to compare.</param>
        /// <returns>True if both positions are equal, otherwise false.</returns>
        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Compares two position objects.
        /// </summary>
        /// <param name="a">The first position to compare.</param>
        /// <param name="b">The second position to compare.</param>
        /// <returns>True if both positions are not equal, otherwise false.</returns>
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Adds two positions to each other.
        /// </summary>
        /// <param name="a">The first position to add.</param>
        /// <param name="b">The second position to add.</param>
        /// <returns>The result of adding two positions to each other.</returns>
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtracts two positions to each other.
        /// </summary>
        /// <param name="a">The first position to subtract.</param>
        /// <param name="b">The second position to subtract.</param>
        /// <returns>The result of subtracting two positions to each other.</returns>
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj != null && (Position)obj == this;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = (hashCode * -1521134295) + X.GetHashCode();
            hashCode = (hashCode * -1521134295) + Y.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Converts position to its string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return $"{PositionConverter.ToString(this)}";
        }
    }
}
