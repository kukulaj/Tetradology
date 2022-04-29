using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Tetradology
{
    abstract public class Tetrad
    {
        static Random rand = new Random(101);
        static int[] permutation = new int[] {0, 1, 2 };

        Fuzz tfuzz;
        Fuzz lfuzz;

        public int range;
        public Vector[] vectors;
        public List<Tetrad> parent;
        public int distance;

        public Tetrad(Vector v)
        {
            range = v.range + 1;
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
                for(int j = 0; j < vectors[i].range; j++)
                {
                    result = result + string.Format("{0} ", vectors[i].power[j]);
                }
                result = result + "> ";

            }
            return result;
        }

        abstract public int shift(int i);

        public double write(StreamWriter file, double t, double pd)
        {
            

            double tstart = t;

            /*
            int slices = 4 * range ;
            double slice = pd / (double)slices;
            */

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
           

           /*

            for (int i = 0; i < slices; i++)
            {
                Random r2 = null;
                double d = slice * tfuzz.noise(t);
                double l = 2500 * lfuzz.noise(3 * t);

                if (i % range == 0)
                {
                    int p1 = rand.Next(permutation.Length);
                    int p2 = (p1 + 1 + rand.Next(permutation.Length - 1)) % permutation.Length;
                    int ps = permutation[p1];
                    permutation[p1] = permutation[p2];
                    permutation[p2] = ps;
                }


                switch (i)
                {
                    default:
                        if(i==0)
                        {
                            
                            r2 = rand;
                        }
                        vectors[order[permutation[i%permutation.Length]]].write(file, t, d, l,0, r2);
                         t += d;
                        break;  
                }
            }
             */

            
            for(int i = 0; i < range; i++)
            {
                vectors[i].write(file, tstart, pd * 0.95, 1200, 1 - i%2, rand);
            }


            //vectors[0].write(file, tstart, pd * 0.95,  1200, -2);

           
            return tstart + pd;
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
