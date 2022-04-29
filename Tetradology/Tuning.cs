using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    
    public class Tuning
    {
        public int edo;
        public int[] bases;
        public int ibad;
        public int lbound;
        public int ubound;
        public Tuning()
        {
          edo = 118;
          bases = new int[] { 3, 5 , 9, 15 };
            ibad =
                    (int)(0.5 + ((double)edo) * Math.Log(15) / Math.Log(2));

            lbound = -2 * edo;
            ubound = 3 * edo;
        }
    }
}
