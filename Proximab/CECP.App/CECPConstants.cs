using System.Diagnostics.CodeAnalysis;

namespace CECP.App
{
    /// <summary>
    /// Represents a set of constants used in all CECP classes.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class CECPConstants
    {
        public const string SendPrefix = "SEND";
        public const string ReceivePrefix = "RECV";
        public const string EnginePrefix = "PRXB";
    }
}
