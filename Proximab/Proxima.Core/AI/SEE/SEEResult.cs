using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.AI.SEE
{
    public class SEEResult
    {
        public PieceType InitialAttackerType { get; set; }
        public PieceType AttackedPieceType { get; set; }

        public Position InitialAttackerFrom { get; set; }
        public Position InitialAttackerTo { get; set; }

        public int Score { get; set; }
    }
}
