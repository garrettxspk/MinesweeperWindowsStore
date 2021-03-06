﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWinStore
{
    class Cell
    {
        private char cellType;

        public char CellType
        {
            get { return cellType; }
            set { cellType = value; }
        }

        private bool isReaveled;

        public bool IsReaveled
        {
            get { return isReaveled; }
            set { isReaveled = value; }
        }

        public Cell(char cellT, bool reveal, CellState s)
        {
            CellType = cellT;
            IsReaveled = reveal;
            State = s;
        }

        private CellState state;

        internal CellState State
        {
          get { return state; }
          set { state = value; }
        }

        public enum CellState
        {
            Blank,
            Flagged,
            Guessed,
            Revealed
        }
    }
}
