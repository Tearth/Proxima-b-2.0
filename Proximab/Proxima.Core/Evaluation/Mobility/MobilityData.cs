namespace Proxima.Core.Evaluation.Mobility
{
    public class MobilityData
    {
        public int WhiteMobility { get; set; }
        public int BlackMobility { get; set; }

        public int Difference
        {
            get { return WhiteMobility - BlackMobility; }
        }
    }
}
