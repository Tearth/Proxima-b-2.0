namespace GUI.ContentDefinitions.Pieces
{
    /// <summary>
    /// Represents information about piece definition.
    /// </summary>
    public class PieceDefinition
    {
        /// <summary>
        /// Gets or sets the name of PieceType enum value.
        /// </summary>
        public string PieceTypeValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the Color enum value.
        /// </summary>
        public string ColorTypeValue { get; set; }

        /// <summary>
        /// Gets or sets the path to the piece texture file.
        /// </summary>
        public string TexturePath { get; set; }
    }
}
