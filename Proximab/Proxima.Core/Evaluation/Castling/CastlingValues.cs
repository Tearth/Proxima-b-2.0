﻿using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Evaluation.Castling
{
    /// <summary>
    /// Represents a set of evaluation parameters for castling evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class CastlingValues
    {
        public static readonly int[] Ratio =
        {
            20,  // Regular
            20   // End
        };
    }
}
