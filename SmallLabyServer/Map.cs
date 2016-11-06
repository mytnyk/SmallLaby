
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace SmallLabyServer
{
    class Map
    {
        private TerrainType[] m_map;
        public int Width { get; }
        public int Height { get; }

        public int StartX { get; }
        public int StartY { get; }

        public int FinishX { get; }
        public int FinishY { get; }

        public TerrainType[] GetFields() { return m_map; }
        public TerrainType GetField(int x, int y) { return m_map[y * Width + x]; }

        public Map()
        {
            var codes = new List<int[]>();
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("SmallLabyServer.Map.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        codes.Add(line.Split(' ').Select(s => Convert.ToInt32(s)).ToArray());
                    }
                }
            }
            Height = codes.Count;
            Width = codes[0].Length;

            m_map = codes.SelectMany(line => line.Select(code => CodeToTerrain(code))).ToArray();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    switch (codes[y][x])
                    {
                        case 2:
                            FinishX = x;
                            FinishY = y;
                            break;
                        case 8:
                            StartX = x;
                            StartY = y;
                            break;
                    }
                }
            }
        }

        private static TerrainType CodeToTerrain(int code)
        {
            switch (code)
            {
                default:
                case 0:
                    return TerrainType.Grass;
                case 1:
                    return TerrainType.Mountain;
            }
        }
    }
}