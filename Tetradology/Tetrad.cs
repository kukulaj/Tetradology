﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tetradology
{
    abstract public class Tetrad
    {
        public const int range = 5;
        public Vector[] vectors;
        public Tetrad parent;

        public Tetrad(Vector v, Tetrad p)
        {
            parent = p;
            vectors = new Vector[range];
            vectors[0] = v;
        }

        public abstract Tetrad step(int i, int j);
        public abstract Tetrad step(int i);

        public string name()
        {
            string result = "";
            for(int i = 0; i< range; i++)
            {
                result = result + "|";
                for(int j = 0; j < Vector.range; j++)
                {
                    result = result + string.Format("{0} ", vectors[i].power[j]);
                }
                result = result + "> ";

            }
            return result;
        }

        public double write(StreamWriter file, double t, double d)
        {
            double result = t + d;
            for(int i = 0; i < range; i++)
            {
                vectors[i].write(file, t, d);
            }

            if (parent != null)
            {
                result = parent.write(file, t + d, d);
            }
            return result;
        }
    }
}
