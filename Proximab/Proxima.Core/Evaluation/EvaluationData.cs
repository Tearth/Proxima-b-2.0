using Proxima.Core.Evaluation.Castling;
using Proxima.Core.Evaluation.KingSafety;
using Proxima.Core.Evaluation.Material;
using Proxima.Core.Evaluation.Mobility;
using Proxima.Core.Evaluation.PawnStructure;
using Proxima.Core.Evaluation.Position;

namespace Proxima.Core.Evaluation
{
    public class EvaluationData
    {
        public MaterialData Material { get; set; }
        public MobilityData Mobility { get; set; }
        public CastlingData Castling { get; set; }
        public PositionData Position { get; set; }
        public PawnStructureData PawnStructure { get; set; }
        public KingSafetyData KingSafety;

        public int Total
        {
            get
            {
                return Material.Difference +
                       Mobility.Difference +
                       Castling.Difference +
                       Position.Difference +
                       PawnStructure.Difference + 
                       KingSafety.Difference;
            }
        }
    }
}
