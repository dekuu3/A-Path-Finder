namespace APathFinder
{
    class Location
    {
        public int X;
        public int Y;
        public int F;
        public int G;
        public int H;
        public Location Parent;

        public Location(int x, int y, int f, int g, int h, Location parent)
        {
            X = x;
            Y = y;
            F = f;
            G = g;
            H = h;
            Parent = parent;
        }
    }
}
