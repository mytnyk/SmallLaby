namespace SmallLabyServer
{
    class Player
    {
        private string m_name;
        public int X { get; set; }
        public int Y { get; set; }

        public int Gold { get; set; }
        // current actual player speed
        public double Speed => BaseSpeed - Penalty;
        private double BaseSpeed => 2;
        private double Penalty => 0.01 * Gold;
        public MovementStrategy MovementStrategy { get; set; }
        public Player(string name)
        {
            MovementStrategy = MovementStrategy.StandStill;
            m_name = name;
        }
    }
}