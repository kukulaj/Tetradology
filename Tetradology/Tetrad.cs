using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Tetradology
{
    abstract public class Tetrad
    {
        static Random rand = new Random(51);

        Fuzz tfuzz;
        Fuzz lfuzz;

        public const int range = 5;
        public Vector[] vectors;
        public List<Tetrad> parent;
        public int distance;

        public Tetrad(Vector v)
        {
            parent = new List<Tetrad>();
            vectors = new Vector[range];
            vectors[0] = new Vector(v);

            tfuzz = new Fuzz(rand, 0.3, 0.015);
            lfuzz = new Fuzz(rand, 0.4, 0.1);

        }

        public abstract Tetrad step(int i, int j);
        public abstract Tetrad step(int i);

        public bool equals(Tetrad t2)
        {
            bool same = true;
            for(int i = 0; i < range; i++)
            {
                same &= vectors[i].equals(t2.vectors[i]);
            }

            return same;
        }

        abstract public Tetrad subtract(Tetrad t2);
        

        public bool check(Tetrad t)
        {
            bool found = false;
            for (int i = 1; i < range; i++)
            {
                Tetrad t2 = step(i);
                found |= t.equals(t2);

                for (int j = i + 1; j < range; j++)
                {
                    t2 = step(i, j);
                    found |= t.equals(t2);
                }
            }
            return found;

        }

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
                pitches[i] = vectors[i].pitch(null);
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
                Random r2 = null;
                double d = slice * tfuzz.noise(t);
                double l = 2500 * lfuzz.noise(3 * t);
                switch (i)
                {
                    default:
                        if(i==0)
                        {
                            r2 = rand;
                        }
                        vectors[rand.Next(range)].write(file, t, d, l,0, r2);
                         t += d;
                        break;  
                }
            }

            vectors[0].write(file, tstart, t - tstart,  2500, -2);

            
            return t;
        }
    
        public void writeVector(StreamWriter file)
        {
            for(int i = 0; i< range; i++)
            {
                vectors[i].writeVector(file);
            }
            file.WriteLine(" ");

           

        }
    }
}
