using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SmallLabyServer
{

    class SmallLabyGlobals
    {
    }
    class SmallLaby : ISmallLaby
    {
        public SmallLaby()
        {
        }
        public int AddPlayer(string name)
        {
            return Game.Instance.AddPlayer(name);
        }

        public int[] GetMap(int player_id)
        {
            var callee = Game.Instance.GetPlayer(player_id);
            var fields = Game.Instance.Map.GetFields();
            var copy = fields.ToArray();

            foreach (var player in Game.Instance.GetPlayers())
            {
                if (player == callee)
                    continue; // do not put player himself onto map
                int index = player.Y * Game.Instance.Map.Width + player.X;
                copy[index] += Map.Enemy;
            }

            return copy;
        }

        public void GetPosition(int player_id, out int x, out int y)
        {
            var player = Game.Instance.GetPlayer(player_id);
            x = player.X;
            y = player.Y;
        }

        public void RemovePlayer(int player_id)
        {
            Game.Instance.RemovePlayer(player_id);
        }

        public void SetPosition(int player_id, int x, int y)
        {
            var player = Game.Instance.GetPlayer(player_id);
            Game.Instance.SetPosition(player, x, y);
        }

        public int GetMapWidth()
        {
            return Game.Instance.Map.Width;
        }

        public int GetMapHeight()
        {
            return Game.Instance.Map.Height;
        }

        public void SetMovementStrategy(int player_id, MovementStrategy strategy)
        {
            var player = Game.Instance.GetPlayer(player_id);
            player.MovementStrategy = strategy;
        }
    }
}
