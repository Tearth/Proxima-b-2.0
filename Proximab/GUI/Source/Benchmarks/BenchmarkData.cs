namespace GUI.Source.Benchmarks
{
    internal class BenchmarkData
    {
        public int TotalNodes { get; set; }
        public int EndNodes { get; set; }
        public float Time { get; set; }

        public BenchmarkData()
        {
            TotalNodes = 0;
            EndNodes = 0;
            Time = 0;
        }
    }
}
