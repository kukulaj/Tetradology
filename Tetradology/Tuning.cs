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
          edo = 27;
          bases = new double[] {1.2, 1.333333, 1.5};
            ibad =
                    (int)(0.5 + ((double)edo) * Math.Log(15) / Math.Log(2));

            lbound = - (3*edo)/2;
            ubound = (5 * edo)/2;
        }
    }
}
