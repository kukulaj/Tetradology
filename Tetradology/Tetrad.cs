using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tetradology
{
    abstract public class Tetrad
    {
        static Random rand = new Random(35);

        public const int range = 4;
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

        public bool branch(Lattice lattice)
        {
            bool found = false;
            for (int i = 1; i < range; i++)
            {
                Tetrad t2 = step(i);
                found |= lattice.insert(t2);

                for (int j = i + 1; j < range; j++)
                {
                    t2 = step(i, j);
                    found |= lattice.insert(t2);
                }
            }
            return found;
        }

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

            int slices = 16;
            double slice = d / (double)slices;


            for (int i = 0; i < 11; i++)
            {
                switch (i)
                {

                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 10:
                    case 1:
                    case 2:
                        vectors[rand.Next(range)].write(file, t, slice);
                         t += slice;
                        break;
                    case 3:
                        vectors[rand.Next(range)].write(file, t, 3 * slice);
                        t += 3 * slice;
                        break;
                    case 9:
                        vectors[0].write(file, t, 3*slice);
                        t += 3*slice;
                        break;

                    case 0:
                        vectors[rand.Next(range)].write(file, t, 2 * slice);
                        t += 2 * slice;
                        break;
                }
            }

            if (parent != null)
            {
                result = parent.write(file, t, d);
            }
            return result;
        }
    }
}
