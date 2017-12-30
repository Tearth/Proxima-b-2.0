using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.AI
{
    public class PVNodesList : List<Move>
    {
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
