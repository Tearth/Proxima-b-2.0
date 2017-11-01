namespace ColorfulConsole
{
    public static class ColorfulConsoleHelpers
    {
        public static string ParseBool(bool value)
        {
            return value ? $"$g{value}$w" : $"$r{value}$w";
        }
    }
}
