using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
        Tetrad tfrom;
        int d;

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

            /*
            for(int i = 0; i < Tetrad.range; i++)
            {
                if(!scale[t2.vectors[i].pitch(null) % scale.Length])
                {
                    return false;
                }
            }
            */
            Tetrad t1 = t2;
            if (tetrads.ContainsKey(st2))
            {
                t1 = tetrads[st2];
            }
            else
            {
                tetrads[st2] = t2;
                toTetrads.Add(t2);
                t2.distance = d;
            }

            t1.parent.Add(tfrom);
            if (st2.Equals(sgoal))
            { 
                Console.WriteLine(string.Format("hit at {0}", d));
               found = true;
                hit = t1;
            }
            
            return found;
        }

        public Tetrad[] walk(Tetrad start, Tetrad tgoal)
        {
            sgoal = tgoal.name();
            string sstart = start.name();

            fromTetrads = new List<Tetrad>();
            toTetrads = new List<Tetrad>();

            d = 0;
            fromTetrads.Add(start);
            start.distance = d;
            tetrads[start.name()] = start;

            hit = null;

            bool found = false;
            int extra = 0;
          
            while(!found /*|| extra < 2*/)
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
                    tfrom = picks[pick];

                    picks[pick] = picks[pcnt - 1];
                    pcnt--;
                    string st = tfrom.name();

                    found |= tfrom.branch(this);
                }

                if(found)
                {
                    extra++;
                }

                fromTetrads = toTetrads;
                toTetrads = new List<Tetrad>();
                
            }
            Tetrad[] path = new Tetrad[d + 1];

            Tetrad scan = hit;
            while(d >= 0)
            {
                path[d] = scan;
                d--;

                if(d >= 0)
                {
                    foreach(Tetrad cp in scan.parent)
                    { 
                        if(cp.distance == d)
                        {
                            scan = cp;
                            break;
                        }
                    }
                }
            }


            return path;
        }
    }
}
