namespace Helpers.ColorfulConsole
{
    /// <summary>
    /// Represents a set of helpers for ColorfulConsole classes.
    /// </summary>
    public static class ColorfulConsoleHelpers
    {
        /// <summary>
        /// Converts bool value into string representation with the appropriate color symbols.
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <returns>
        /// Green "True" if the value is true or red "False" if the value is false.
        /// </returns>
        public static string ParseBool(bool value)
        {
            return value ? $"$g{value}$w" : $"$r{value}$w";
        }
    }
}
