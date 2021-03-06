﻿using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Commons.Pieces
{
    /// <summary>
    /// Represents the piece types.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum PieceType
    {
        Pawn = 0,
        Knight = 1,
        Bishop = 2,
        Rook = 3,
        Queen = 4,
        King = 5
    }
}
