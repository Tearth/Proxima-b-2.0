namespace Proxima.Core.MoveGenerators.MagicBitboards.Generators
{
    public class FieldPattern
    {
        public ulong Occupancy { get; private set; }
        public ulong Attacks { get; private set; }

        public FieldPattern()
        {

        }

        public FieldPattern(ulong occupancy, ulong attacks)
        {
            Occupancy = occupancy;
            Attacks = attacks;
        }
    }
}
