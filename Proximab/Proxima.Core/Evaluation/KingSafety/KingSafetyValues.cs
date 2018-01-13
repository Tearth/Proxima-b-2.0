using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Evaluation.KingSafety
{
    /// <summary>
    /// Represents a set of evaluation parameters for king safety evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public class KingSafetyValues
    {
        public static readonly int[] AttackedNeighboursRatio =
        {
            -7,   // Regular
            -2    // End
        };
    }
}
