using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class Lattice
    {
        Dictionary<string, Tetrad> tetrads;
        List<Tetrad> fromTetrads;
        List<Tetrad> toTetrads;
        Tetrad hit;
        string sgoal;
        public Lattice()
        {
            tetrads = new Dictionary<string, Tetrad>();
        }

        private bool insert(Tetrad t2)
        {
            bool found = false;
            string st2 = t2.name();
            if (!tetrads.ContainsKey(st2))
            {
                tetrads[st2] = t2;
                toTetrads.Add(t2);
                if (st2.Equals(sgoal))
                {
                    found = true;
                    hit = t2;
                }
            }
            return found;
        }

        public Tetrad walk(Tetrad tgoal)
        {
            sgoal = tgoal.name();

            fromTetrads = new List<Tetrad>();
            toTetrads = new List<Tetrad>();

            Tetrad start = new OTetrad(new Vector(), null);
            fromTetrads.Add(start);
            tetrads[start.name()] = start;

            hit = null;

            bool found = false;
            int d = 0;
            while(!found)
            {
                d++;
                Console.WriteLine(string.Format("step from {0} tetrads", fromTetrads.Count));
                foreach (Tetrad t in fromTetrads)
                {
                    string st = t.name();
                    for (int i = 1; i < Tetrad.range; i++)
                    {
                        Tetrad t2 = t.step(i);
                        found |= insert(t2);

                        for (int j = i + 1; j < Tetrad.range; j++)
                        {
                            t2 = t.step(i, j);
                            found |= insert(t2);
                        }
                    } 
                }

                fromTetrads = toTetrads;
                toTetrads = new List<Tetrad>();
                
            }
            Console.WriteLine(string.Format("hit at {0}", d));

            return hit;
        }
    }
}
