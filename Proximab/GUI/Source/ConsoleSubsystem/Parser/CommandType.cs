namespace GUI.Source.ConsoleSubsystem.Parser
{
    internal enum CommandType
    {
        None,
        Invalid,
        Help,
        Colors,
        SaveBoard,
        LoadBoard,
        AddPiece,
        Occupancy,
        BenchmarkMoves,
        IsCheck
    }
}
