

namespace SmallLabyServer
{
    class Map
    {
        private TerrainType[] m_map =
        {
              TerrainType.Grass   , TerrainType.Grass   , TerrainType.Mountain, TerrainType.Mountain, TerrainType.Mountain,
              TerrainType.Mountain, TerrainType.Grass   , TerrainType.Grass   , TerrainType.Grass   , TerrainType.Grass   ,
              TerrainType.Mountain, TerrainType.Mountain, TerrainType.Grass   , TerrainType.Mountain, TerrainType.Grass   ,
              TerrainType.Mountain, TerrainType.Mountain, TerrainType.Grass   , TerrainType.Grass   , TerrainType.Grass   ,
        };
        public int Width { get; } = 5;
        public int Height { get; } = 4;

        public TerrainType[] GetFields() { return m_map; }
        public TerrainType GetField(int x, int y) { return m_map[y * Width + x]; }
    }
}