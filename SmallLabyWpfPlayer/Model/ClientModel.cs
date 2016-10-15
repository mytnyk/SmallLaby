using SmallLabyWpfPlayer.ServiceRefSmallLaby;
using System.Collections.Generic;
using System.Linq;

namespace SmallLabyWpfPlayer
{
    public class ClientModel
    {
        public enum Terrain
        {
            Grass,
            Mountain,
            Water,
            Forest,
            Unknown,
        }
        public enum MovementStrategy
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            DoNotMove,
            MoveRandom,
        }
        public struct Player
        {
            public int X;
            public int Y;
            public string Name;
            public bool Me;
        }
        public struct Field
        {
            public int X;
            public int Y;
            public Terrain Terrain;
        }
        private SmallLabyClient m_client;
        private int m_player_id;
        private string m_player_name;
        private TerrainType[] m_map;
        private int m_map_width;
        private int m_map_height;

        public ClientModel()
        {
            m_client = new SmallLabyClient();
            m_map = m_client.GetMap();
            m_map_width = m_client.GetMapWidth();
            m_map_height = m_client.GetMapHeight();
            IsConnected = false;
        }

        public void Connect(string player_name)
        {
            m_player_id = m_client.AddPlayer(player_name);
            m_player_name = player_name;
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                m_client.RemovePlayer(m_player_id);
                IsConnected = false;
            }
        }

        public IEnumerable<Player> GetPlayers()
        {
            return m_client.GetPlayers().Select(p =>
            new Player
            {
                X = p.X,
                Y = p.Y,
                Name = (p.Id == m_player_id) ? m_player_name : "enemy",
                Me = p.Id == m_player_id
            });
        }

        public IEnumerable<Field> GetMap()
        {
            for (int y = 0; y < m_map_height; y++)
            {
                for (int x = 0; x < m_map_width; x++)
                {
                    var field = m_map[y * m_map_width + x];

                    Terrain terrain = Terrain.Unknown;
                    switch (field)
                    {
                        case TerrainType.Grass:
                            terrain = Terrain.Grass;
                            break;
                        case TerrainType.Mountain:
                            terrain = Terrain.Mountain;
                            break;
                        case TerrainType.Water:
                            terrain = Terrain.Water;
                            break;
                        case TerrainType.Forest:
                            terrain = Terrain.Forest;
                            break;
                    }
                    yield return new Field { X = x, Y = y, Terrain = terrain };
                }
            }
        }

        public void SetMovementStrategy(MovementStrategy strategy)
        {
            switch (strategy)
            {
                case MovementStrategy.MoveLeft:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.MoveLeft);
                    break;
                case MovementStrategy.MoveRight:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.MoveRight);
                    break;
                case MovementStrategy.MoveUp:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.MoveUp);
                    break;
                case MovementStrategy.MoveDown:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.MoveDown);
                    break;
                case MovementStrategy.DoNotMove:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.StandStill);
                    break;
                case MovementStrategy.MoveRandom:
                    m_client.SetMovementStrategy(m_player_id,
                        ServiceRefSmallLaby.MovementStrategy.RandomDirection);
                    break;
            }
        }

        public bool IsConnected { get; private set; }
    }
}
