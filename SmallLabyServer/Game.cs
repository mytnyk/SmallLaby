using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallLabyServer
{
    class Game
    {
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

        private Game ()
        {

        }

        public static Game Instance { get; } = new Game();
    }
}
