using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWinStore
{
    class HighScore
    {
        private string diff;

        public string Difficulty
        {
            get { return diff; }
            set { diff = value; }
        }

        private int time;

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        private int place;

        public int Place
        {
            get { return place; }
            set { place = value; }
        }
    }
}
