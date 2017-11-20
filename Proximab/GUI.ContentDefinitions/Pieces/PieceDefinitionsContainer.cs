using System.Collections.Generic;

namespace GUI.ContentDefinitions.Pieces
{
    /// <summary>
    /// Container od piece definitions (required by MonoGame Pipeline Tool).
    /// </summary>
    public class PieceDefinitionsContainer
    {
        /// <summary>
        /// Gets or sets the piece definitions list.
        /// </summary>
        public List<PieceDefinition> Definitions { get; set; }
    }
}
