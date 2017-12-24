using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECP.App.ConsoleSubsystem
{
    /// <summary>
    /// Represents available CECP command types.
    /// </summary>
    public enum CommandType
    {
        Unrecognised,
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
        Quit
    }
}
