using System;
using Proxima.Core.MoveGenerators.Moves;

namespace GUI.App.PromotionSubsystem
{
    /// <summary>
    /// Represents the PromotionSelected event arguments. 
    /// </summary>
    public class PromotionSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the promotion move.
        /// </summary>
        public PromotionMove Move { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="move">The promotion move.</param>
        public PromotionSelectedEventArgs(PromotionMove move)
        {
            Move = move;
        }
    }
}
