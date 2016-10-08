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
        private Dictionary<int, Player> m_players = new Dictionary<int, Player>();

        public IEnumerable<Player> GetPlayers()
        {
            return m_players.Values;
        }
        public int AddPlayer(string name)
        {
            var p = new Player(name);
            int player_id = m_current_player_id;
            m_players[player_id] = p;
            m_current_player_id++;
            return player_id;
        }

        public void RemovePlayer(int player_id)
        {
            m_players.Remove(player_id);
        }

        public Player GetPlayer(int player_id)
        {
            return m_players[player_id];
        }

        private Game()
        {
            var move_playes_thread = new Thread(MovePlayers);
            move_playes_thread.Start();
        }

        public static Game Instance { get; } = new Game();

        private static void MovePlayers()
        {
            Instance._MovePlayers();
        }
        private void _MovePlayers()
        {
            var random_generator = new Random();
            while (!GameOver)
            {
                Thread.Sleep(500);
                var players = GetPlayers();
                foreach (var player in players)
                {
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
        }

        public void SetPosition(Player player, int x, int y)
        {
            if (x < 0 || y < 0 ||
                x >= Instance.Map.Width || y >= Instance.Map.Height)
                return;

            int field = Instance.Map.GetField(x, y);
            if ((field & Map.Wall) == 0)
            {
                player.X = x;
                player.Y = y;
            }
        }
    }
}
