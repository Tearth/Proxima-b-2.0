using Proxima.Core.Commons.Colors;

namespace Proxima.Core.Heuristics
{
    public class HeuristicResult
    {
        public int[] Material { get; set; }

        public int Total
        {
            get
            {
                return (Material[(int)Color.White] - Material[(int)Color.Black]);
            }
        }
    }
}
