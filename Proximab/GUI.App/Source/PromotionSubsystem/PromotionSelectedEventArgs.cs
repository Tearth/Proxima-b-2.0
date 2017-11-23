using System;
using Proxima.Core.Commons.Moves;

namespace GUI.App.Source.PromotionSubsystem
{
    /// <summary>
    /// Represents the PromotionSelected event arguments. 
    /// </summary>
    internal class PromotionSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the promotion move.
        /// </summary>
        public PromotionMove Move { get; private set; }

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
