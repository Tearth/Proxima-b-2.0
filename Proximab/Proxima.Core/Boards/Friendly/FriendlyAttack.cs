using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a single attack in the user-friendly way.
    /// </summary>
    public class FriendlyAttack
    {
        /// <summary>
        /// Gets the piece color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Gets the source piece position.
        /// </summary>
        public Position From { get; private set; }

        /// <summary>
        /// Gets the destination piece position.
        /// </summary>
        public Position To { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyAttack"/> class.
        /// </summary>
        public FriendlyAttack()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyAttack"/> class.
        /// </summary>
        /// <param name="color">The piece color.</param>
        /// <param name="from">The source piece position.</param>
        /// <param name="to">The destination piece position.</param>
        public FriendlyAttack(Color color, Position from, Position to)
        {
            Color = color;
            From = from;
            To = to;
        }
    }
}
