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
          edo = 171;
          bases = new int[] { 3, 5 , 7 , 9 };
        }
    }
}
