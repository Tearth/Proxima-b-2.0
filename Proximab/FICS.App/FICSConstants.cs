namespace FICS.App
{
    /// <summary>
    /// Represents a set of constants used in all FICS classes.
    /// </summary>
    public static class FICSConstants
    {
        public const string EndOfLine = "\n\r";

        public const string SendPrefix = "SEND";
        public const string ReceivePrefix = "RECV";
        public const string EnginePrefix = "PRXB";

        public const string LoginCommand = "login:";
        public const string PasswordCommand = "password:";
        public const string Prompt = "fics%";

        public const string ServerAddressConfigKeyName = "ServerAddress";
        public const string ServerPortConfigKeyName = "ServerPort";
    }
}
