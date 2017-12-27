namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents available CECP command types.
    /// </summary>
    public enum CommandType
    {
        Unrecognized,
        XBoard,
        ProtoVer,
        Accepted,
        Rejected,
        Ping,
        New,
        Post,
        NoPost,
        Time,
        OTim,
        White,
        Black,
        Go,
        UserMove,
        Quit,
        Result
    }
}
