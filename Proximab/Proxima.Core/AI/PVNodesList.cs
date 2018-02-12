using System.Collections.Generic;
using System.Text;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Represents a list of PV nodes.
    /// </summary>
    public class PVNodesList : List<Move>
    {
        /// <summary>
        /// Gets a string human-readable representation of PV nodes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var pvNodesStringBuilder = new StringBuilder();
            foreach (var move in this)
            {
                pvNodesStringBuilder.Append(move);
                pvNodesStringBuilder.Append(" ");
            }

            if (pvNodesStringBuilder.Length > 0)
            {
                pvNodesStringBuilder.Remove(pvNodesStringBuilder.Length - 1, 1);
            }

            return pvNodesStringBuilder.ToString();
        }
    }
}