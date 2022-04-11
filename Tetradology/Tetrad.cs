using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tetradology
{
    abstract public class Tetrad
    {
        static Random rand = new Random(47);

        Fuzz tfuzz;
        Fuzz lfuzz;

        public const int range = 4;
        public Vector[] vectors;
        public Tetrad parent;

        public Tetrad(Vector v, Tetrad p)
        {
            parent = p;
            vectors = new Vector[range];
            vectors[0] = v;

            tfuzz = new Fuzz(rand, 0.3);
            lfuzz = new Fuzz(rand, 0.4);

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

        public double write(StreamWriter file, double t, double pd)
        {
            if (parent == null)
                return t;

            double tstart = t;

            int slices = 16;
            double slice = pd / (double)slices;

            int[] pitches = new int[range];
            int[] order = new int[range];

            for (int i = 0; i < range; i++)
            {
                order[i] = i;
                pitches[i] = vectors[i].pitch();
            }

            for(int i = 0; i< range; i++)
            {
                for(int j = i+1; j < range; j++)
                {
                    if(pitches[i]>pitches[j])
                    {
                        int pswap = pitches[j];
                        pitches[j] = pitches[i];
                        pitches[i] = pswap;

                        int oswap = order[j];
                        order[j] = order[i];
                        order[i] = oswap;
                    }
                }
            }
           

           

            for (int i = 0; i < slices; i++)
            {
                double d = slice * tfuzz.noise(t);
                double l = 2500 * lfuzz.noise(t);
                switch (i)
                {
                    default:
                        vectors[rand.Next(range)].write(file, t, d, l);
                         t += d;
                        break;  
                }
            }

            vectors[0].write(file, tstart, t - tstart,  2500, -2);

            if (parent != null)
            {
                t = parent.write(file, t, pd);
            }
            return t;
        }
    
        public void writeVector(StreamWriter file)
        {
            for(int i = 0; i< range; i++)
            {
                vectors[i].writeVector(file);
            }
            file.WriteLine(" ");

            if (parent != null)
            {
                parent.writeVector(file);
            }

        }
    }
}
