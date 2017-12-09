namespace GUI.App.Source.CommandsSubsystem
{
    /// <summary>
    /// Represents available type of commands.
    /// </summary>
    internal enum CommandType
    {
        None,
        Invalid,
        Help,
        Colors,
        SaveBoard,
        LoadBoard,
        AddPiece,
        RemovePiece,
        Occupancy,
        Attacks,
        MovesTest,
        Check,
        Castling,
        Evaluation,
        Hash,
        BestMove,
        Reset,
        Mode,
        RunAIGame
    }
}
