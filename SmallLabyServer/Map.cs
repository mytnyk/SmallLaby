

namespace SmallLabyServer
{
    class Map
    {
        static public int Road = 0x01; // 0001
        static public int Wall = 0x02; // 0010
        static public int Monster = 0x04; // 0100
        static public int Enemy = 0x08; // 1000

        private int[] m_map =
        {
              Road, Road, Wall, Wall, Wall,
              Wall, Road, Road, Road, Road,
              Wall, Wall, Road, Wall, Road,
              Wall, Wall, Road, Road, Road,
        };
        public int Width { get; } = 5;
        public int Height { get; } = 4;

        public int[] GetFields() { return m_map; }
        public int GetField(int x, int y) { return m_map[y*Width + x]; }
    }
}