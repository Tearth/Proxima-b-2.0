using System;
using System.Collections.Generic;

namespace Helpers.ColorfulConsole
{
    /// <summary>
    /// Represents a color definitions container.
    /// </summary>
    public static class ColorDefinitions
    {
        public static readonly Dictionary<char, ConsoleColor> Colors = new Dictionary<char, ConsoleColor>
        {
            { 'w', ConsoleColor.White },
            { 'r', ConsoleColor.Red },
            { 'R', ConsoleColor.DarkRed },
            { 'g', ConsoleColor.Green },
            { 'G', ConsoleColor.DarkGreen },
            { 'b', ConsoleColor.Blue },
            { 'B', ConsoleColor.DarkBlue },
            { 'm', ConsoleColor.Magenta },
            { 'y', ConsoleColor.Yellow },
            { 'c', ConsoleColor.Cyan }
        };
    }
}