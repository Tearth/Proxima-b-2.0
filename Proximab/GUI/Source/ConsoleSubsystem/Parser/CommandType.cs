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
        RemovePiece,
        Occupancy,
        TestMoves,
        IsCheck
    }
}
