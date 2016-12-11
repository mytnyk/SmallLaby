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
        public enum ItemType
        {
            Gold,
            Bonus,
            Exit,
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
        public struct Monster
        {
            public int X;
            public int Y;
        }
        public struct Field
        {
            public int X;
            public int Y;
            public Terrain Terrain;
        }
        public struct Item
        {
            public int X;
            public int Y;
            public ItemType Type;
        }

        private SmallLabyClient m_client;
        private int m_player_id;
        private string m_player_name;
        private TerrainType[] m_map;

      public int MapWidth { get; }
      public int MapHeight { get; }

        public ClientModel()
        {
            m_client = new SmallLabyClient();
            m_map = m_client.GetMap();
            MapWidth = m_client.GetMapWidth();
            MapHeight = m_client.GetMapHeight();
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

        public IEnumerable<Monster> GetMonsters()
        {
            return m_client.GetMonsters().Select(m =>
            new Monster
            {
                X = m.X,
                Y = m.Y,
            });
        }

        public IEnumerable<Item> GetItems()
        {
            foreach (var item_info in m_client.GetItems())
            {
                ItemType type = ItemType.Unknown;
                switch (item_info.Item)
                {
                    case ServiceRefSmallLaby.Item.Gold:
                        type = ItemType.Gold;
                        break;
                    case ServiceRefSmallLaby.Item.Bonus:
                        type = ItemType.Bonus;
                        break;
                    case ServiceRefSmallLaby.Item.Exit:
                        type = ItemType.Exit;
                        break;
                }
                yield return new Item
                {
                    X = item_info.X,
                    Y = item_info.Y,
                    Type = type,
                };
            }
        }
        public IEnumerable<Field> GetMap()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    var field = m_map[y * MapWidth + x];

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
            if (!IsConnected)
                return;
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

        public int PlayerGold => IsConnected ? m_client.GetGold(m_player_id) : 0;

        public double PlayerSpeed => IsConnected ? m_client.GetSpeed(m_player_id) : 0;

    }
}
