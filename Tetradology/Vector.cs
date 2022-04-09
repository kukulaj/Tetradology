using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class Vector
    {
        public int[] power;
        public const int range = 3;
        public Vector()
        {
            power = new int[range];
        }
        public Vector(Vector v)
        {
            power = new int[range];

            for(int i = 0; i < range; i++)
            {
                power[i] = v.power[i];
            }


        }
    }
}
