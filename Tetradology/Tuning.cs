using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    
    public class Tuning
    {
        public int edo;
        public double[] bases;
        public int ibad;
        public int lbound;
        public int ubound;
        public Tuning()
        {
          edo = 72;
          bases = new double[] {3, 7, 15};
            ibad =
                    (int)(0.5 + ((double)edo) * Math.Log(15) / Math.Log(2));

            lbound = - edo / 2;
            ubound = (5 * edo)/2;
        }
    }
}
