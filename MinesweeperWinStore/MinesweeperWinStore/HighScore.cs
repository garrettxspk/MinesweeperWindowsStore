using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWinStore
{
    class HighScore
    {
        private string initials;

        public string Initials
        {
            get { return initials; }
            set 
            { 
                if(value.Length > 3)
                {
                    initials = value.Substring(0, 3);
                }
                initials = initials.ToUpper();
            }
        }

        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
    }
}
