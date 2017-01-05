using System;
using System.Linq;
using System.Collections.Generic;

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
            player.PrimaryMovementStrategy = strategy;
        }

        public PlayerInfo[] GetPlayers()
        {
            Console.WriteLine("GetPlayers..." + DateTime.Now);
            return Game.Instance.Players.Select(
                p => new PlayerInfo { Id = p.Key, X = p.Value.X, Y = p.Value.Y }).ToArray();
        }

        public ItemInfo[] GetItems()
        {
            Console.WriteLine("GetItems..." + DateTime.Now);
            return Game.Instance.GoldItems.Select(
                i => new ItemInfo { Item = Item.Gold, X = i.X, Y = i.Y }).
                Concat(new List<ItemInfo> {
                    new ItemInfo
                    {
                        Item = Item.Exit,
                        X = Game.Instance.Map.FinishX,
                        Y = Game.Instance.Map.FinishY
                    } }).ToArray();
        }

        public int GetGold(int player_id)
        {
            var player = Game.Instance.GetPlayer(player_id);
            return player.Gold;
        }

        public double GetSpeed(int player_id)
        {
            var player = Game.Instance.GetPlayer(player_id);
            return player.Speed;
        }

        public MonsterInfo[] GetMonsters()
        {
            Console.WriteLine("GetMonsters..." + DateTime.Now);
            return Game.Instance.Monsters.Select(
                m => new MonsterInfo { Health = m.Health, X = m.X, Y = m.Y}).ToArray();
        }
    }
}
