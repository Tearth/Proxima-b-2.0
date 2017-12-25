using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public AIResult AIResult { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThinkingOutputEventArgs"/> class.
        /// </summary>
        /// <param name="text">The message content.</param>
        public ThinkingOutputEventArgs(AIResult aiResult)
        {
            AIResult = aiResult;
        }
    }
}
