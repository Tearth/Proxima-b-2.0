namespace Proxima.Core.MoveGenerators.MagicBitboards
{
    /// <summary>
    /// Represents a single attack pattern for the specified field. By pattern, we mean a pair of occupancy and
    /// attacks variables.
    /// </summary>
    /// <remarks>
    /// For Occupancy=10110010 and 00000X00 (where X is the localization of field), the Attacks property
    /// will have 00011110. The most edge bits in Attacks property are always zero (to save memory when
    /// magic keys are generated).
    /// </remarks>
    public class FieldAttackPattern
    {
        /// <summary>
        /// Gets the occupancy value.
        /// </summary>
        public ulong Occupancy { get; }

        /// <summary>
        /// Gets the attacks value.
        /// </summary>
        public ulong Attacks { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAttackPattern"/> class.
        /// </summary>
        /// <param name="occupancy">The occupancy value (where 1 means that the specified field is occupied).</param>
        /// <param name="attacks">The attacks value (where 1 means that the specified field can be attacked by the piece.</param>
        public FieldAttackPattern(ulong occupancy, ulong attacks)
        {
            Occupancy = occupancy;
            Attacks = attacks;
        }
    }
}
