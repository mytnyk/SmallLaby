using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmallLabyPlayer.ServiceRefSmallLaby;

namespace SmallLabyPlayer
{
    class Program
    {
        class FieldWithPlayer
        {
            public TerrainType Terrain;
            public bool HasPlayer;
            public int PlayerId;
            public override string ToString()
            {
                string text = Terrain.ToString();
                if (HasPlayer)
                {
                    text += "(" + PlayerId + ")";
                }
                return text;
            }
        }
        static void ShowMap(FieldWithPlayer[] map_with_players, int width, int height)
        {
            Console.Clear();
            var sb = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var field = map_with_players[y * width + x];
                    sb.Append(field);
                    sb.Append('\t');
                }
                sb.Append("\n");
            }
            Console.Write(sb);
        }
        static void Main(string[] args)
        {
            var client = new SmallLabyClient();

            int player_id = client.AddPlayer("alex");

            var map = client.GetMap();
            var width = client.GetMapWidth();
            var height = client.GetMapHeight();
            while (true)
            {
                var players = client.GetPlayers();

                var map_with_players = map.Select(t => new FieldWithPlayer { Terrain = t, HasPlayer = false, PlayerId = 0 }).ToArray();

                foreach (var player in players)
                {
                    var field = map_with_players[player.X + player.Y * width];
                    field.HasPlayer = true;
                    field.PlayerId = player.Id;
                }

                ShowMap(map_with_players, width, height);

                bool user_wants_to_exit = false;
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        user_wants_to_exit = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        client.SetMovementStrategy(player_id, MovementStrategy.MoveLeft);
                        break;
                    case ConsoleKey.RightArrow:
                        client.SetMovementStrategy(player_id, MovementStrategy.MoveRight);
                        break;
                    case ConsoleKey.DownArrow:
                        client.SetMovementStrategy(player_id, MovementStrategy.MoveDown);
                        break;
                    case ConsoleKey.UpArrow:
                        client.SetMovementStrategy(player_id, MovementStrategy.MoveUp);
                        break;
                }
                if (user_wants_to_exit)
                    break;
            }
            client.RemovePlayer(player_id);
            client.Close();
        }
    }
}
