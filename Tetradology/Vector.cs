using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Tetradology
{
    public class Vector
    {
        public int[] power;
        public const int range = 2;
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
            double[] primes = new double[] {3, 5, 7, 11, 13, 17 };
            int edo = 118;

            int pitch = 0;
            for(int i = 0; i < range; i++)
            {
                pitch += power[i] * (int)(0.5 + ((double)edo)*Math.Log(primes[i])/Math.Log(2) );
            }


            pitch = pitch % edo;
            if(pitch < 0)
            {
                pitch += edo;
            }
            double freq = 2.0 * Math.Exp(Math.Log(2) * ((double) pitch) / ((double)edo));

            file.WriteLine(string.Format("i3  {0} {1} {2} 2500", t, d, freq));
        }
    }
}
