﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWinStore
{
    class BoardConfiguration
    {
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int numberOfMines;

        public int NumberOfMines
        {
            get { return numberOfMines; }
            set { numberOfMines = value; }
        }

        public BoardConfiguration(int h, int w, int m)
        {
            height = h;
            width = w;
            numberOfMines = m;
        }
    }
}
