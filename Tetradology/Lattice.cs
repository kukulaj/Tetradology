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
        public Random rand;

        bool[] scale;
        public Lattice(Random pr)
        {
            rand = pr ;
            tetrads = new Dictionary<string, Tetrad>();

            scale = new bool[1];
            scale[0] = true;
           // for(int i = 0; i < 47; i++)
           // {
            //   scale[(100 + i * 11) % scale.Length] = true;
          //}

        }

        public bool insert(Tetrad t2)
        {
            bool found = false;
            string st2 = t2.name();

            for(int i = 0; i < Tetrad.range; i++)
            {
                if(!scale[t2.vectors[i].pitch(null) % scale.Length])
                {
                    return false;
                }
            }

            if (tetrads.ContainsKey(st2))
                {
                    return false;
                }

            tetrads[st2] = t2;
            toTetrads.Add(t2);
            if (st2.Equals(sgoal))
            {
                found = true;
                hit = t2;
            }
            
            return found;
        }

        public Tetrad walk(Tetrad start, Tetrad tgoal)
        {
            sgoal = tgoal.name();

            fromTetrads = new List<Tetrad>();
            toTetrads = new List<Tetrad>();

          
            fromTetrads.Add(start);
            tetrads[start.name()] = start;

            hit = null;

            bool found = false;
            int extra = -1;
            int d = 0;
            while(!found)
            {
                found = false;
                d++;
                Console.WriteLine(string.Format("step from {0} tetrads", fromTetrads.Count));

                int pcnt = fromTetrads.Count;
               Tetrad[] picks = new Tetrad[pcnt];
                int ti = 0;
                foreach (Tetrad t in fromTetrads)
                {
                    picks[ti] = t;
                    ti++;
                }
               
                while(pcnt > 0)
                {
                    int pick = rand.Next(pcnt);
                    Tetrad t = picks[pick];

                    picks[pick] = picks[pcnt - 1];
                    pcnt--;
                    string st = t.name();

                    found |= t.branch(this);
                }

                if(found)
                {
                    extra++;
                }

                fromTetrads = toTetrads;
                toTetrads = new List<Tetrad>();
                
            }
            Console.WriteLine(string.Format("hit at {0}", d));

            return hit;
        }
    }
}
