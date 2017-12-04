using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators
{
    /// <summary>
    /// Represents an interface for all attack generators.
    /// </summary>
    public interface IAttacksGenerator
    {
        /// <summary>
        /// Generates list of the field patterns for the specified field.
        /// </summary>
        /// <param name="fieldIndex">The field index.</param>
        /// <returns>The list of generated patterns for the specified field.</returns>
        List<FieldAttackPattern> Generate(int fieldIndex);
    }
}
