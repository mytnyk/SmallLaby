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

        public TerrainType[] GetMap()
        {
            return Game.Instance.Map.GetFields();
        }
        /*
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
        }*/

        public void RemovePlayer(int player_id)
        {
            Game.Instance.RemovePlayer(player_id);
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

        public PlayerInfo[] GetPlayers()
        {
            return Game.Instance.GetPlayers().Select(
                p => new PlayerInfo { Id = p.Key, X = p.Value.X, Y = p.Value.Y }).ToArray();
        }

        public ItemInfo[] GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
