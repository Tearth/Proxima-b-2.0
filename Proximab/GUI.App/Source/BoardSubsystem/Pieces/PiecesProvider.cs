using GUI.ContentDefinitions.Pieces;
using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Colors;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Proxima.Core.Boards.Friendly;

namespace GUI.App.Source.BoardSubsystem.Pieces
{
    internal class PiecesProvider
    {
        Dictionary<int, Texture2D> _pieceTextures;

        public PiecesProvider()
        {
            _pieceTextures = new Dictionary<int, Texture2D>();
        }

        public void LoadContent(ContentManager contentManager)
        {
            var pieceDefinitionsContainer = contentManager.Load<PieceDefinitionsContainer>("XML\\PieceDefinitions");

            foreach(var pieceDefinition in pieceDefinitionsContainer.Definitions)
            {
                var piece = (PieceType)Enum.Parse(typeof(PieceType), pieceDefinition.PieceTypeValue);
                var color = (Color)Enum.Parse(typeof(Color), pieceDefinition.ColorTypeValue);
                var pieceTexture = contentManager.Load<Texture2D>(pieceDefinition.TexturePath);

                var hash = GetFriendlyPieceHash(new FriendlyPiece(piece, color));
                _pieceTextures.Add(hash, pieceTexture);
            }
        }

        public Texture2D GetPieceTexture(FriendlyPiece piece)
        {
            var hash = GetFriendlyPieceHash(piece);
            return _pieceTextures[hash];
        }

        int GetFriendlyPieceHash(FriendlyPiece piece)
        {
            return ((int)piece.Color * 100) + (int)piece.Type;
        }
    }
}
