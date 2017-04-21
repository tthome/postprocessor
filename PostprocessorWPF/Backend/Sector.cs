namespace PostprocessorWPF.Backend
{
    internal class Sector
    {
        public Sector(int id, int x1, int x2, int y1, int y2, int z1, int z2)
        {
            Id = id;
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            Z1 = z1;
            Z2 = z2;
        }

        public int Id { get; set; }

        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public int Z1 { get; set; }
        public int Z2 { get; set; }
    }
}