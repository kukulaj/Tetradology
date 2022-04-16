using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Tetradology
{
    public class Vector
    {
        static int offset = 0;
        public int[] power;
        public const int range = 3;
        public const int edo = 441;
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

        public bool equals(Vector v2)
        {
            bool same = true;
            for(int i = 0; i<range; i++)
            {
                same &= (power[i] == v2.power[i]);
            }
            return same;
        }

        public void subtract(Vector v2)
        {
            for(int i = 0;  i<range; i++)
            {
                power[i] -= v2.power[i];
            }
        }

        public int pitch(Random rand)
        {
            double[] primes = new double[] { 3, 5, 7, 11, 13, 17 };
           

            int p = 0;
            for (int i = 0; i < range; i++)
            {
                p += power[i] * (int)(0.5 + ((double)edo) * Math.Log(primes[i]) / Math.Log(2));
            }


            p = p % edo;

            int bottom = 0;

            if(rand != null)
            {
                
                if (rand.NextDouble() < 0.1)
                { 
                    if(rand.Next(2)==0)
                    { 
                        if(offset > 0)
                        {
                            offset--;
                        }
                    }
                    else 
                    {
                        if (offset < edo)
                        {
                            offset++;
                        }
                    }
                }
                bottom = offset;
            }

            while (p < bottom)
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
            write(file, t, d, l, shift, null);
        }
        public void write(StreamWriter file, double t, double d, double l, int shift, Random rand)
        {
            int p = pitch(rand);
            p += shift * edo;
            double freq = 2.0 * Math.Exp(Math.Log(2) * ((double) p) / ((double)edo));

            file.WriteLine(string.Format("i3  {0} {1} {2} {3}", t, d, freq, l));
        }

        public void writeVector(StreamWriter file)
        {
            int p = pitch(null);
            
            file.Write(string.Format("({0}):|", p));

            for(int i = 0; i < range; i++)
            {
                file.Write(string.Format("{0} ", power[i]));
            }
            file.Write(">; ");
        }
    }
}
