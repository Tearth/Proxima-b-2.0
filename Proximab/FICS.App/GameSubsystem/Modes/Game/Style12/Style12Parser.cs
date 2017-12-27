using System;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;

namespace FICS.App.GameSubsystem.Modes.Game.Style12
{
    /// <summary>
    /// Represents a set of methods to parse Style12 responses received from FICS.
    /// </summary>
    public class Style12Parser
    {
        private const int MinimalStyle12TokensCount = 31;

        /// <summary>
        /// Parses FICS response to object.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The Style12 container with parsed data.</returns>
        public Style12Container Parse(string text)
        {
            var style12Container = new Style12Container();
            var splitInput = text.Split(' ');

            if (splitInput.Length < MinimalStyle12TokensCount)
            {
                return null;
            }

            style12Container.StyleID = splitInput[0];
            style12Container.BoardState = ParseBoardState(splitInput);
            style12Container.ColorToMove = GetColorType(splitInput[9]);
            style12Container.DoublePawnPush = Convert.ToInt32(splitInput[10]);

            style12Container.CastlingPossible[(int)Color.White, (int)CastlingType.Short] = splitInput[11] == "1";
            style12Container.CastlingPossible[(int)Color.White, (int)CastlingType.Long] = splitInput[12] == "1";
            style12Container.CastlingPossible[(int)Color.Black, (int)CastlingType.Short] = splitInput[13] == "1";
            style12Container.CastlingPossible[(int)Color.Black, (int)CastlingType.Long] = splitInput[14] == "1";

            style12Container.Rule50Moves = Convert.ToInt32(splitInput[15]);
            style12Container.GameNumber = Convert.ToInt32(splitInput[16]);

            style12Container.PlayerName[(int)Color.White] = splitInput[17];
            style12Container.PlayerName[(int)Color.Black] = splitInput[18];

            style12Container.Relation = GetRelationType(splitInput[19]);

            style12Container.InitialTime = Convert.ToInt32(splitInput[20]);
            style12Container.IncrementalTime = Convert.ToInt32(splitInput[21]);

            style12Container.MaterialStrength[(int)Color.White] = Convert.ToInt32(splitInput[22]);
            style12Container.MaterialStrength[(int)Color.Black] = Convert.ToInt32(splitInput[23]);

            style12Container.RemainingTime[(int)Color.White] = Convert.ToInt32(splitInput[24]);
            style12Container.RemainingTime[(int)Color.Black] = Convert.ToInt32(splitInput[25]);

            style12Container.MovesToMade = Convert.ToInt32(splitInput[26]);
            style12Container.PreviousMove = GetStyle12Move(splitInput[27], style12Container.EnemyColor);
            style12Container.TimeOfPreviousMove = splitInput[28];
            style12Container.PrettyPreviousMoveNotation = splitInput[29];

            style12Container.BoardOrientation = GetBoardOrientationType(splitInput[30]);

            return style12Container;
        }

        /// <summary>
        /// Parses Style12 board state to array.
        /// </summary>
        /// <param name="splitInput">The list of Style12 tokens.</param>
        /// <returns>The array of the board state ranks.</returns>
        private string[] ParseBoardState(string[] splitInput)
        {
            var boardState = new string[8];

            for (int i = 0; i < 8; i++)
            {
                boardState[i] = splitInput[i + 1];
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
