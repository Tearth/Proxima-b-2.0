using ContentDefinitions.Pieces;
using Core.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Source.BoardSubsystem.Pieces
{
    internal class PiecesProvider
    {
        Dictionary<PieceType, Texture2D> _pieceTextures;

        public PiecesProvider()
        {
            _pieceTextures = new Dictionary<PieceType, Texture2D>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            var pieceDefinitionsContainer = contentManager.Load<PieceDefinitionsContainer>("XML\\PieceDefinitions");

            foreach(var pieceDefinition in pieceDefinitionsContainer.Definitions)
            {
                var pieceType = (PieceType)Enum.Parse(typeof(PieceType), pieceDefinition.EnumTypeValue);
                var pieceTexture = contentManager.Load<Texture2D>(pieceDefinition.TexturePath);

                _pieceTextures.Add(pieceType, pieceTexture);
            }
        }

        public Texture2D GetPieceTexture(PieceType pieceType)
        {
            return _pieceTextures[pieceType];
        }
    }
}
