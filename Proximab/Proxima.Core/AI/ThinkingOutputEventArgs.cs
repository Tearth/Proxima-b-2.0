using System;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Represents information about DataReceived event
    /// </summary>
    public class ThinkingOutputEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the AI result for the specified depth.
        /// </summary>
        public AiResult AiResult { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThinkingOutputEventArgs"/> class.
        /// </summary>
        /// <param name="aiResult">The AI result.</param>
        public ThinkingOutputEventArgs(AiResult aiResult)
        {
            AiResult = aiResult;
        }
    }
}
