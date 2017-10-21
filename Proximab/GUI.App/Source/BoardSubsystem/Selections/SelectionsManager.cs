using Proxima.Core.Boards;
using Proxima.Core.Commons.Positions;
using GUI.App.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.App.Source.BoardSubsystem.Selections
{
    internal class SelectionsManager
    {
        Texture2D _internalSelection;
        Texture2D _externalSelection;

        List<Selection> _selections;

        public SelectionsManager()
        {
            _selections = new List<Selection>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _internalSelection = contentManager.Load<Texture2D>("Textures\\InternalSelection");
            _externalSelection = contentManager.Load<Texture2D>("Textures\\ExternalSelection");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var selection in _selections)
            {
                Texture2D texture = null;
                switch (selection.Type)
                {
                    case SelectionType.Internal: { texture = _internalSelection; break; }
                    case SelectionType.External: { texture = _externalSelection; break; }
                }

                var x = (selection.Position.X - 1) * Constants.FieldWidthHeight;
                var y = (8 - selection.Position.Y) * Constants.FieldWidthHeight;

                var position = new Vector2(x, y);

                spriteBatch.Draw(texture, position + Constants.BoardPosition, Constants.FieldSize, Microsoft.Xna.Framework.Color.White);
            }
        }

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

        public Position SelectField(Point clickPoint, FriendlyBoard board)
        {
            var fieldX = (int)((clickPoint.X - Constants.BoardPosition.X) / Constants.FieldWidthHeight) + 1;
            var fieldY = 8 - (int)((clickPoint.Y - Constants.BoardPosition.Y) / Constants.FieldWidthHeight);

            fieldX = Math.Min(8, fieldX);
            fieldY = Math.Min(8, fieldY);

            fieldX = Math.Max(1, fieldX);
            fieldY = Math.Max(1, fieldY);

            var position = new Position(fieldX, fieldY);
            _selections.Add(new Selection(position, SelectionType.Internal));

            return position;
        }

        public Selection GetInternalSelection()
        {
            return _selections.FirstOrDefault(p => p.Type == SelectionType.Internal);
        }

        public void RemoveAllSelections()
        {
            _selections.Clear();
        }
    }
}
