﻿using System;
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

        public bool insert(Tetrad t2)
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

                    found |= t.branch(this);
                }

                fromTetrads = toTetrads;
                toTetrads = new List<Tetrad>();
                
            }
            Console.WriteLine(string.Format("hit at {0}", d));

            return hit;
        }
    }
}
