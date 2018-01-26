using System;
using System.Collections.Generic;
using System.Linq;
using GUI.App.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons.Positions;

namespace GUI.App.BoardSubsystem.Selections
{
    /// <summary>
    /// Represents a set of methods to manage selections on the board.
    /// </summary>
    /// <remarks>
    /// There are two types of selection:
    ///  - internal - every selection made by clicking the left mouse button.
    ///  - external - every other selections added via AddExternalSelections method
    /// </remarks>
    public class SelectionsManager
    {
        private Texture2D _internalSelection;
        private Texture2D _externalSelection;

        private List<Selection> _selections;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionsManager"/> class.
        /// </summary>
        public SelectionsManager()
        {
            _selections = new List<Selection>();
        }

        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="contentManager">Monogame content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _internalSelection = contentManager.Load<Texture2D>("Textures\\InternalSelection");
            _externalSelection = contentManager.Load<Texture2D>("Textures\\ExternalSelection");
        }

        /// <summary>
        /// Draws the board axes.
        /// </summary>
        /// <param name="spriteBatch">Monogame sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var selection in _selections)
            {
                Texture2D texture = null;
                switch (selection.Type)
                {
                    case SelectionType.Internal:
                    {
                        texture = _internalSelection;
                        break;
                    }

                    case SelectionType.External:
                    {
                        texture = _externalSelection;
                        break;
                    }
                }

                var position = new Vector2
                {
                    X = (selection.Position.X - 1) * Constants.FieldWidthHeight,
                    Y = (8 - selection.Position.Y) * Constants.FieldWidthHeight
                };

                spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Color.White);
            }
        }

        /// <summary>
        /// Adds the external selections.
        /// </summary>
        /// <param name="selections">The list of external selections.</param>
        public void AddExternalSelections(IEnumerable<Position> selections)
        {
            foreach (var position in selections)
            {
                if (!_selections.Exists(p => p.Type == SelectionType.External && p.Position == position))
                {
                    _selections.Add(new Selection(position, SelectionType.External));
                }
            }
        }

        /// <summary>
        /// Adds an internal selection by calculating the click point and converting it to the board position.
        /// </summary>
        /// <param name="clickPoint">Position of mouse cursor when left button has been clicked.</param>
        /// <returns>Board position of selected field.</returns>
        public Position SelectField(Point clickPoint)
        {
            var fieldPosition = GetFieldPosition(clickPoint);
            var normalizedPosition = NormalizePosition(fieldPosition);

            _selections.Add(new Selection(normalizedPosition, SelectionType.Internal));

            return normalizedPosition;
        }

        /// <summary>
        /// Searches the internal selections.
        /// </summary>
        /// <returns>The internal selection if exists, otherwise null.</returns>
        public Selection GetInternalSelection()
        {
            return _selections.FirstOrDefault(p => p.Type == SelectionType.Internal);
        }

        /// <summary>
        /// Removes all (internal and external) selections.
        /// </summary>
        public void RemoveAllSelections()
        {
            _selections.Clear();
        }

        /// <summary>
        /// Calculates a field position based on the click point.
        /// </summary>
        /// <param name="clickPoint">The mouse click point.</param>
        /// <returns>The field position.</returns>
        private Position GetFieldPosition(Point clickPoint)
        {
            var x = 1 + (int)((clickPoint.X - Constants.BoardPosition.X) / Constants.FieldWidthHeight);
            var y = 8 - (int)((clickPoint.Y - Constants.BoardPosition.Y) / Constants.FieldWidthHeight);

            return new Position(x, y);
        }

        /// <summary>
        /// Normalizes the position to board standards (from 1 to 8).
        /// </summary>
        /// <param name="position">The position to normalize.</param>
        /// <returns>The normalized position.</returns>
        private Position NormalizePosition(Position position)
        {
            position.X = Math.Min((byte)8, position.X);
            position.Y = Math.Max((byte)1, position.Y);

            return position;
        }
    }
}
