using System;
using System.Collections.Generic;
using GUI.ContentDefinitions.Pieces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;

namespace GUI.App.BoardSubsystem.Pieces
{
    /// <summary>
    /// Represents a set of methods for loading and getting piece textures.
    /// </summary>
    public class PiecesProvider
    {
        private Dictionary<int, Texture2D> _pieceTextures;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecesProvider"/> class.
        /// </summary>
        public PiecesProvider()
        {
            _pieceTextures = new Dictionary<int, Texture2D>();
        }

        /// <summary>
        /// Loads the resources. Must be called before first use of any other class method.
        /// </summary>
        /// <param name="contentManager">Monogame content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            var pieceDefinitionsContainer = contentManager.Load<PieceDefinitionsContainer>("XML\\PieceDefinitions");

            foreach (var pieceDefinition in pieceDefinitionsContainer.Definitions)
            {
                var piece = (PieceType)Enum.Parse(typeof(PieceType), pieceDefinition.PieceTypeValue);
                var color = (Color)Enum.Parse(typeof(Color), pieceDefinition.ColorTypeValue);
                var pieceTexture = contentManager.Load<Texture2D>(pieceDefinition.TexturePath);

                var hash = GetFriendlyPieceHash(color, piece);
                _pieceTextures.Add(hash, pieceTexture);
            }
        }

        /// <summary>
        /// Searches a texture for the specified piece.
        /// </summary>
        /// <param name="color">The piece color</param>
        /// <param name="type">The piece type</param>
        /// <returns>The texture for the specified piece.</returns>
        public Texture2D GetPieceTexture(Color color, PieceType type)
        {
            var hash = GetFriendlyPieceHash(color, type);
            return _pieceTextures[hash];
        }

        /// <summary>
        /// Calculates a hash for the specified piece color and type.
        /// </summary>
        /// <param name="color">The piece color</param>
        /// <param name="piece">The piece type</param>
        /// <returns>The hash for the specified piece.</returns>
        private int GetFriendlyPieceHash(Color color, PieceType piece)
        {
            return (int)color * 100 + (int)piece;
        }
    }
}
