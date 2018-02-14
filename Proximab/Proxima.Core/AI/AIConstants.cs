﻿using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.AI
{
    /// <summary>
    /// Represents a set of AI constants.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class AIConstants
    {
        public const int MateValue = 100000;

        public const int InitialAlphaValue = -999999999;
        public const int InitialBetaValue = -InitialAlphaValue;

        public const int MaxDepth = 12;

        public const int KillerHeuristicSlotsCount = 3;

        public const int MinimalDepthToStartHelperThreads = 3;
    }
}
