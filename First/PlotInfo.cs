namespace First
{
    internal class PlotInfo
    {
        public PlotInfo(float @from, float to, float a, float b, float c, float ratio)
        {
            From = @from;
            To = to;
            A = a;
            B = b;
            C = c;
            Ratio = ratio;
        }

        public float From { get; set; }
        public float To { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }

        public float Ratio { get; set; }

        public float SegmentLength { get { return To - From; } }
        public float ValuesSegmentLength { get { return SegmentLength / Ratio; } }
    }
}