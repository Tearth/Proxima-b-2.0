using System.Collections.Generic;

namespace Proxima.Core.MoveGenerators.MagicBitboards.Attacks.Generators
{
    public interface IAttacksGenerator
    {
        List<FieldPattern> Generate(int fieldIndex);
    }
}
