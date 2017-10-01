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

        }
    }
}
