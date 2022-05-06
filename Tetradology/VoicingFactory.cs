using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class VoicingFactory
    {
        Voicing prev;
        Voicing next;
        Tetrad tetrad;
        Voicing best = null;
        int sbest = 0;
        Random rand;

        public VoicingFactory(Tetrad t, Voicing p, Random pr)
        {
            rand = pr;
            tetrad = t;
            prev = p;
            next = null;
        }

        public VoicingFactory(Tetrad t, Voicing p, Voicing n, Random pr)
        {
            rand = pr;
            tetrad = t;
            prev = p;
            next = n;
        }

        public Voicing getBest()
        {
            int[] order = new int[tetrad.range];
            bool below = false;
            for(int ibelow = 0; ibelow < 2; ibelow++)
            {
                below = !below;
                permute(order, 0, below);
            }
            return best;
        }

        public void permute(int[] order, int i, bool below)
        {
            for(int j = 0; j < tetrad.range; j++)
            {
                bool okay = true;
                for(int p = 0; p < i; p++)
                {
                    if(order[p] == j)
                    {
                        okay = false;
                    }
                }

                if (okay)
                {
                    order[i] = j;
                    if (i < tetrad.range - 1)
                    {
                        permute(order, i + 1, below);
                    }
                    else
                    {
                        Voicing tv = new Voicing(tetrad, prev, order, below);
                        if (tv.valid())
                        {
                            int score = tv.distance(prev);
                            if(next != null)
                            {
                                score += tv.distance(next);
                            }
                            if (best == null 
                                || score < sbest 
                                || (score == sbest && rand.Next(2)==0))
                            {
                                best = tv;
                                sbest = score;
                            }
                        }
                    }
                }
            }
        }

    }
}
