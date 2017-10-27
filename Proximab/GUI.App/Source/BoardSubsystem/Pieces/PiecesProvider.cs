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

                var hash = GetFriendlyPieceHash(color, piece);
                _pieceTextures.Add(hash, pieceTexture);
            }
        }

        public Texture2D GetPieceTexture(FriendlyPiece piece)
        {
            var hash = GetFriendlyPieceHash(piece.Color, piece.Type);
            return _pieceTextures[hash];
        }

        int GetFriendlyPieceHash(Color color, PieceType piece)
        {
            return ((int)color * 100) + (int)piece;
        }
    }
}
