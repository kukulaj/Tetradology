using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Tetradology
{
    public class Vector
    {
        public int[] power;
        public const int range = 4;
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

        public void write(StreamWriter file, double t, double d)
        {
            int pitch = (200 * power[0] + 110 * power[1] + 276 * power[2] + 157*power[3]) % 342;
            if(pitch < 0)
            {
                pitch += 342;
            }
            double freq = 2.0 * Math.Exp(Math.Log(2) * ((double) pitch) / ((double)342));

            file.WriteLine(string.Format("i3  {0} {1} {2} 2500", t, d, freq));
        }
    }
}
