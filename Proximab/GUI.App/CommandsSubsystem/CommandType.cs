﻿using System.Diagnostics.CodeAnalysis;

namespace GUI.App.CommandsSubsystem
{
    /// <summary>
    /// Represents available type of commands.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public enum CommandType
    {
        None,
        Invalid,
        Help,
        Exit,
        Colors,
        SaveBoard,
        LoadBoard,
        AddPiece,
        RemovePiece,
        Occupancy,
        Attacks,
        MovesTest,
        Check,
        Mate,
        Stalemate,
        Castling,
        Evaluation,
        Hash,
        BestMove,
        Reset,
        Mode,
        RunAIGame,
        Quiescence,
        SEE,
        Repetition,
        Reversible,
        RunGame
    }
}
