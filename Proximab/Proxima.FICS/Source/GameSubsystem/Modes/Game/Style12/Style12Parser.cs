using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.FICS.Source.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents a set of methods to parse Style12 responses received from FICS.
    /// </summary>
    public class Style12Parser
    {
        /// <summary>
        /// Parses FICS response to object.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The Style12 container with parsed data.</returns>
        public Style12Container Parse(string text)
        {
            var style12Container = new Style12Container();
            var splittedInput = text.Split(' ');

            if (splittedInput.Length < 31)
            {
                return null;
            }

            style12Container.StyleID = splittedInput[0];
            style12Container.BoardState = ParseBoardState(splittedInput);
            style12Container.ColorToMove = GetColorType(splittedInput[9]);
            style12Container.DoublePawnPush = Convert.ToInt32(splittedInput[10]);

            style12Container.WhiteShortCastlingPossible = splittedInput[11] == "1";
            style12Container.WhiteLongCastlingPossible = splittedInput[12] == "1";
            style12Container.BlackShortCastlingPossible = splittedInput[13] == "1";
            style12Container.BlackLongCastlingPossible = splittedInput[14] == "1";

            style12Container.Rule50Moves = Convert.ToInt32(splittedInput[15]);
            style12Container.GameNumber = Convert.ToInt32(splittedInput[16]);

            style12Container.WhitePlayerName = splittedInput[17];
            style12Container.BlackPlayerName = splittedInput[18];

            style12Container.Relation = GetRelationType(splittedInput[19]);

            style12Container.InitialTime = Convert.ToInt32(splittedInput[20]);
            style12Container.IncrementalTime = Convert.ToInt32(splittedInput[21]);
            style12Container.WhiteMaterialStrength = Convert.ToInt32(splittedInput[22]);
            style12Container.BlackMaterialStrength = Convert.ToInt32(splittedInput[23]);
            style12Container.WhiteRemainingTime = Convert.ToInt32(splittedInput[24]);
            style12Container.BlackRemainingTime = Convert.ToInt32(splittedInput[25]);

            style12Container.MovesToMade = Convert.ToInt32(splittedInput[26]);
            style12Container.PreviousMove = GetStyle12Move(splittedInput[27], style12Container.EnemyColor);
            style12Container.TimeOfPreviousMove = splittedInput[28];
            style12Container.PrettyPreviousMoveNotation = splittedInput[29];

            style12Container.BoardOrientation = GetBoardOrientationType(splittedInput[30]);

            return style12Container;
        }

        /// <summary>
        /// Parses Style12 board state to array.
        /// </summary>
        /// <param name="splittedInput">The list of Style12 tokens.</param>
        /// <returns>The array of the board state ranks.</returns>
        private string[] ParseBoardState(string[] splittedInput)
        {
            var boardState = new string[8];

            for (int i = 0; i < 8; i++)
            {
                boardState[i] = splittedInput[i + 1];
            }

            return boardState;
        }

        /// <summary>
        /// Gets the color type by parsing the color symbol
        /// </summary>
        /// <param name="color">The color symbol</param>
        /// <returns>The color type.</returns>
        private Color GetColorType(string color)
        {
            var colorChar = Convert.ToChar(color);
            return ColorConverter.GetColor(colorChar);
        }

        /// <summary>
        /// Gets the move object basing on the Style12 move.
        /// </summary>
        /// <param name="text">The Style12 move text to parse.</param>
        /// <param name="color">The color of the player making the move.</param>
        /// <returns>The Style12 move object.</returns>
        private Style12Move GetStyle12Move(string text, Color color)
        {
            var style12MoveParser = new Style12MoveParser();
            return style12MoveParser.Parse(text, color);
        }

        /// <summary>
        /// Gets the relation type basing on its Style12 id.
        /// </summary>
        /// <param name="relation">The Style12 relation id.</param>
        /// <returns>The relation type.</returns>
        private Style12RelationType GetRelationType(string relation)
        {
            return (Style12RelationType)Convert.ToInt32(relation);
        }

        /// <summary>
        /// Gets the board orientation type basing on its Style12 is.
        /// </summary>
        /// <param name="boardOrientation">The style12 board orientation id.</param>
        /// <returns>The board orientation type.</returns>
        private Style12OrientationType GetBoardOrientationType(string boardOrientation)
        {
            return (Style12OrientationType)Convert.ToInt32(boardOrientation);
        }
    }
}
