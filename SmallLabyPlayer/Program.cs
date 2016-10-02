using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmallLabyPlayer.ServiceRefSmallLaby;

namespace SmallLabyPlayer
{
    class Program
    {
        static void ShowMap(int[] map, int width, int height)
        {
            Console.Clear();
            var sb = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append(map[y * width + x]);
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

            while (true)
            {
                int[] map = client.GetMap();

                ShowMap(map, client.GetMapWidth(), client.GetMapHeight());

                int x, y;
                x = client.GetPosition(player_id, out y);

                bool user_wants_to_exit = false;
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        user_wants_to_exit = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        x--;
                        break;
                    case ConsoleKey.RightArrow:
                        x++;
                        break;
                    case ConsoleKey.DownArrow:
                        y++;
                        break;
                    case ConsoleKey.UpArrow:
                        y--;
                        break;
                }
                if (user_wants_to_exit)
                    break;

                client.SetPosition(x, y, player_id);
            }
            client.RemovePlayer(player_id);
            client.Close();
        }
    }
}
