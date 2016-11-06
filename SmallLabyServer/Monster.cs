using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallLabyServer
{
    class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }//health is given in percents (%)

        public double Speed { get; } = 2;

        public MovementStrategy MovementStrategy { get; } = MovementStrategy.RandomDirection;
        public Monster()
        {
            Health = 100;
        }
    }
}
