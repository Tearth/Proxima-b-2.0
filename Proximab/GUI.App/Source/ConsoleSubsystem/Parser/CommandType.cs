namespace GUI.App.Source.ConsoleSubsystem.Parser
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
        Attacks,
        MovesTest,
        Check,
        Castling
    }
}
