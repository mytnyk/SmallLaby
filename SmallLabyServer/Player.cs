namespace SmallLabyServer
{
    class Player
    {
        private string m_name;
        public int X { get; set; }
        public int Y { get; set; }
        public Player(string name)
        {
            m_name = name;
        }
    }
}