﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Tetradology
{
    public class Vector
    {
        public int[] power;
        public const int range = 3;
        public const int edo = 171;
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

        public int pitch()
        {
            double[] primes = new double[] { 3, 5, 7, 11, 13, 17 };
           

            int p = 0;
            for (int i = 0; i < range; i++)
            {
                p += power[i] * (int)(0.5 + ((double)edo) * Math.Log(primes[i]) / Math.Log(2));
            }


            p = p % edo;
            if (p < 0)
            {
                p += edo;
            }
            return p;
        }

        public void write(StreamWriter file, double t, double d, double l)
        {
            write(file, t, d, l, 0);
        }
        public void write(StreamWriter file, double t, double d, double l, int shift)
        {
            int p = pitch();
            p += shift * edo;
            double freq = 2.0 * Math.Exp(Math.Log(2) * ((double) p) / ((double)edo));

            file.WriteLine(string.Format("i3  {0} {1} {2} {3}", t, d, freq, l));
        }

        public void writeVector(StreamWriter file)
        {
            int p = pitch();
            
            file.Write(string.Format("({0}):|", p));

            for(int i = 0; i < range; i++)
            {
                file.Write(string.Format("{0} ", power[i]));
            }
            file.Write(">; ");
        }
    }
}
