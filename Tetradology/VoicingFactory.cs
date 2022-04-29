﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class VoicingFactory
    {
        Voicing prev;
        Tetrad tetrad;
        Voicing best = null;
        int sbest = 0;

        public VoicingFactory(Tetrad t, Voicing p)
        {
            tetrad = t;
            prev = p;
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
                            if (best == null || score < sbest)
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