namespace Proxima.Core.Evaluation.Calculators
{
    public class MaterialResult
    {
        public int WhiteMaterial { get; set; }
        public int BlackMaterial { get; set; }

        public int Difference
        {
            get { return WhiteMaterial - BlackMaterial; }
        }
    }
}
