using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SmallLabyServer
{
    class Game
    {
        public bool GameOver = false;
        public Map Map { get; } = new Map();

        private int m_current_player_id = 100;
        public Dictionary<int, Player> Players { get; } = new Dictionary<int, Player>();
        public List<Gold> GoldItems { get; } = new List<Gold>();

        public int AddPlayer(string name)
        {
            var p = new Player(name);
            int player_id = m_current_player_id;
            Players[player_id] = p;
            m_current_player_id++;

            var move_player_thread = new Thread(MovePlayer);
            move_player_thread.Start(player_id);

            return player_id;
        }

        public void RemovePlayer(int player_id)
        {
            var player = GetPlayer(player_id);
            var gold = player.Gold;
            if (gold > 0)
                GoldItems.Add(new Gold { Amount = gold, X = player.X, Y = player.Y });
            Players.Remove(player_id);
        }

        public Player GetPlayer(int player_id)
        {
            return Players[player_id];
        }

        public bool TryGetPlayer(int player_id, out Player player)
        {
            return Players.TryGetValue(player_id, out player);
        }

        private Game()
        {
            GoldItems.Add(new Gold { Amount = 25, X = 1, Y = 1});
            GoldItems.Add(new Gold { Amount = 50, X = 3, Y = 3 });
        }

        public static Game Instance { get; } = new Game();

        private static void MovePlayer(object player_id)
        {
            Instance._MovePlayer((int)player_id);
        }
        private void _MovePlayer(int player_id)
        {
            var random_generator = new Random();
            while (!GameOver)
            {
                Player player;
                if (!TryGetPlayer(player_id, out player))
                    return;

                Thread.Sleep((int)(1000 / player.Speed)); // problem if speed is zero

                if (!TryGetPlayer(player_id, out player))
                    return;

                var x = player.X;
                var y = player.Y;
                switch (player.MovementStrategy)
                {
                    case MovementStrategy.StandStill:
                        break;
                    case MovementStrategy.MoveDown:
                        y++;
                        break;
                    case MovementStrategy.MoveUp:
                        y--;
                        break;
                    case MovementStrategy.MoveLeft:
                        x--;
                        break;
                    case MovementStrategy.MoveRight:
                        x++;
                        break;
                    case MovementStrategy.RandomDirection:
                        int random_move = random_generator.Next(5);
                        switch (random_move)
                        {
                            case 0:
                                y++;
                                break;
                            case 1:
                                y--;
                                break;
                            case 2:
                                x++;
                                break;
                            case 3:
                                x--;
                                break;
                            case 4:
                                break;
                        }
                        break;
                }
                SetPosition(player, x, y);
            }
        }

        private void SetPosition(Player player, int x, int y)
        {
            if (x < 0 || y < 0 ||
                x >= Instance.Map.Width || y >= Instance.Map.Height)
                return;

            var terrain = Instance.Map.GetField(x, y);
            if (terrain != TerrainType.Mountain)
            {
                player.X = x;
                player.Y = y;

                var gold = GoldItems.Find(g => g.X == x && g.Y == y);
                if (gold != null)
                {
                    player.Gold += gold.Amount;
                    GoldItems.Remove(gold);
                }
            }
        }
    }
}
