namespace SmallLabyServer
{
    class Player
    {
        private string m_name;
        public int X { get; set; }
        public int Y { get; set; }
        public double Speed { get; set; }
        public MovementStrategy MovementStrategy { get; set; }
        public Player(string name)
        {
            Speed = 2;
            MovementStrategy = MovementStrategy.StandStill;
            m_name = name;
        }
    }
}