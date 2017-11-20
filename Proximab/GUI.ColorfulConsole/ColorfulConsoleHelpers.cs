namespace GUI.ColorfulConsole
{
    /// <summary>
    /// Represents a set of helpersfor ColorfulConsole classes.
    /// </summary>
    public static class ColorfulConsoleHelpers
    {
        /// <summary>
        /// Returns green "True" if the value is true or red "False" if the value is false.
        /// Suprising.
        /// </summary>
        public static string ParseBool(bool value)
        {
            return value ? $"$g{value}$w" : $"$r{value}$w";
        }
    }
}
