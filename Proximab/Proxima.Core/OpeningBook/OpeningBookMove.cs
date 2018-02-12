using Proxima.Core.Commons.Positions;

namespace Proxima.Core.OpeningBook
{
    /// <summary>
    /// Represents a simple opening move.
    /// </summary>
    public class OpeningBookMove
    {
        /// <summary>
        /// Gets or sets the source move position.
        /// </summary>
        public Position From { get; set; }

        /// <summary>
        /// Gets or sets the target move position.
        /// </summary>
        public Position To { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningBookMove"/> class.
        /// </summary>
        public OpeningBookMove(Position from, Position to)
        {
            From = from;
            To = to;
        }
    }
}
