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
    private Random m_random_generator = new Random();
    public bool GameOver = false;
    public Map Map { get; } = new Map();

    private int m_current_player_id = 100;
    public Dictionary<int, Player> Players { get; } = new Dictionary<int, Player>();
    public List<Gold> GoldItems { get; } = new List<Gold>();

    public List<Monster> Monsters { get; } = new List<Monster>();
    public int AddPlayer(string name)
    {
      var p = new Player(name);
      p.X = Map.StartX;
      p.Y = Map.StartY;
      lock (Players)
      {
        int player_id = m_current_player_id;
        Players[player_id] = p;
        m_current_player_id++;

        var move_player_thread = new Thread(MovePlayer);
        move_player_thread.Start(player_id);

        return player_id;
      }
    }

    public void RemovePlayer(int player_id)
    {
      lock (Players)
      {
        var player = GetPlayer(player_id);
        var gold = player.Gold;
        if (gold > 0)
        {
          GoldItems.Add(new Gold { Amount = gold, X = player.X, Y = player.Y });
          player.Gold = 0;
        }
        Players.Remove(player_id);
      }
    }

    public Player GetPlayer(int player_id)
    {
      return Players[player_id];
    }

    public bool TryGetPlayer(int player_id, out Player player)
    {
      return Players.TryGetValue(player_id, out player);
    }

    public void AddMonsters()
    {
      Monsters.Add(new Monster { X = 3, Y = 3 });

      var move_monsters_thread = new Thread(MoveMonsters);
      move_monsters_thread.Start();
    }
    private Game()
    {
      GoldItems.Add(new Gold { Amount = 25, X = 1, Y = 1 });
      GoldItems.Add(new Gold { Amount = 50, X = 3, Y = 3 });
      GoldItems.Add(new Gold { Amount = 25, X = 2, Y = 2 });

      AddMonsters();
    }

    public static Game Instance { get; } = new Game();

    private void ChangeXY(ref int x, ref int y, MovementStrategy strategy)
    {
      switch (strategy)
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
          int random_move = m_random_generator.Next(5);
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
    }
    private static void MoveMonsters()
    {
      Instance._MoveMonsters();
    }
    private void _MoveMonsters()
    {
      while (!GameOver)
      {
        Thread.Sleep(500);
        foreach (var monster in Monsters)
        {
          var x = monster.X;
          var y = monster.Y;
          ChangeXY(ref x, ref y, monster.MovementStrategy);
          SetPosition(monster, x, y);
        }
      }
    }
    private static void MovePlayer(object player_id)
    {
      Instance._MovePlayer((int)player_id);
    }
    private void _MovePlayer(int player_id)
    {
      while (!GameOver)
      {
        Player player;
        if (!TryGetPlayer(player_id, out player))
          return;

        Thread.Sleep((int)(1000 / player.Speed)); // problem if speed is zero

        lock (Players)
        {
          if (!TryGetPlayer(player_id, out player))
            return;

          var x = player.X;
          var y = player.Y;
          ChangeXY(ref x, ref y, player.PrimaryMovementStrategy);

          if (SetPosition(player, x, y))
          {
            player.SecondaryMovementStrategy = player.PrimaryMovementStrategy;
          }
          else
          {
            x = player.X;
            y = player.Y;
            ChangeXY(ref x, ref y, player.SecondaryMovementStrategy);
            SetPosition(player, x, y);
          }
        }
      }
    }

    private bool SetPosition(Player player, int x, int y)
    {
      if (x < 0 || y < 0 ||
          x >= Map.Width || y >= Map.Height)
        return false;

      var terrain = Map.GetField(x, y);
      if (terrain == TerrainType.Mountain)
        return false;

        player.X = x;
        player.Y = y;

        var gold = GoldItems.Find(g => g.X == x && g.Y == y);
        if (gold != null)
        {
          player.Gold += gold.Amount;
          GoldItems.Remove(gold);
        }

        var monster = Monsters.Find(m => m.X == x && m.Y == y);
        if (monster != null)
        {
          KillPlayer(player);
        }

        if (x == Map.FinishX && y == Map.FinishY)
        {
          System.Console.Write("Finish");
        }

      return true;
    }
    private void SetPosition(Monster monster, int x, int y)
    {
      if (x < 0 || y < 0 ||
          x >= Map.Width || y >= Map.Height)
        return;

      var terrain = Map.GetField(x, y);
      if (terrain != TerrainType.Mountain)
      {
        monster.X = x;
        monster.Y = y;

        lock (Players)
        {
          var players = Players.Values.Where(p => p.X == x && p.Y == y);
          foreach (var player in players)
          {
            KillPlayer(player);
          }
        }
      }
    }

    private void KillPlayer(Player player)
    {
      var gold = player.Gold;
      if (gold > 0)
      {
        GoldItems.Add(new Gold { Amount = gold, X = player.X, Y = player.Y });
        player.Gold = 0;
      }
      player.X = Map.StartX;
      player.Y = Map.StartY;
    }
  }
}
