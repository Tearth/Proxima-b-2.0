namespace GUI.App.CommandsSubsystem
{
    /// <summary>
    /// Represents available type of commands.
    /// </summary>
    public enum CommandType
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
        SEE
    }
}
