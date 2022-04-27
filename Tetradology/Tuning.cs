using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    
    public class Tuning
    {
        public int edo;
        public int[] bases;
        public Tuning()
        {
          edo = 118;
          bases = new int[] { 3, 5, 9, 15 };
        }
    }
}
