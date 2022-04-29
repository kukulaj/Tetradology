using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Tetradology
{
    public class Voicing
    {
        Tetrad tetrad;
        int[] octaves;
        int[] order;
        Tuning tuning;
        int[] pitches;

        public Voicing(Tetrad pt)
        {
            tetrad = pt;
            tuning = tetrad.vectors[0].tuning;
            octaves = new int[tetrad.range];
            order = new int[tetrad.range];

            for (int vi = 0; vi < tetrad.range; vi++)
            {
                order[vi] = vi;
                if (vi == 0)
                {
                    octaves[0] = 0;
                }
                else
                {
                    int pdiff = pitch(vi-1) - pitch(vi);
                    octaves[vi] = pdiff / tuning.edo;
                    if (pdiff < 0)
                    {
                        octaves[vi]--;
                    }
                    octaves[vi]++;
                }

            }

            Debug.Assert(valid());
            savePitches();
        }
        public Voicing(Tetrad pt, Voicing prev, int[] permute, bool below)
        {
            tetrad = pt;
            tuning = tetrad.vectors[0].tuning;
            octaves = new int[tetrad.range];
            order = new int[tetrad.range];

            for(int vi = 0; vi < tetrad.range; vi++)
            {
                order[vi] = permute[vi];
                if(vi==0)
                {
                    int pdiff = prev.pitch(0) - pitch(0);
                    octaves[0] = pdiff / tuning.edo;
                    if(pdiff < 0)
                    {
                        octaves[0]--;
                    }

                    if(!below)
                    {
                        octaves[0]++;
                    }
                }
                else 
                {
                    int pdiff = pitch(vi-1) - pitch(vi);
                    octaves[vi] = pdiff / tuning.edo;
                    if (pdiff < 0)
                    {
                        octaves[vi]--;
                    }
                    octaves[vi]++;
                }

            }
            savePitches();
        }

        public void savePitches()
        {
            pitches = new int[tetrad.range];
            for(int i = 0; i < tetrad.range; i++)
            {
                pitches[i] = pitch(i);
            }
        }

        public bool forbidden(int p1, int p2)
        {
            bool bad = false;

            int edo = tetrad.vectors[0].tuning.edo;
            if (((p2 - p1 - tuning.ibad) % edo == 0) && p2 - p1 > edo)
            {
                bad = true;
            }

            return bad;
        }
        public bool valid()
        {
            bool okay = true;

            if (pitch(0) < tuning.lbound
                  || pitch(tetrad.range - 1) > tuning.ubound)
                return false;

            for(int v1 = 0; v1 < tetrad.range; v1++)
            {
                for (int v2 = v1+1; v2 < tetrad.range; v2++)
                {
                    if(forbidden(pitch(v1), pitch(v2)))
                    {
                        okay = false;
                    }
                }
            }
            return okay;
        }

        public int pitch(int vi)
        {
            return tetrad.vectors[order[vi]].spitch() + tuning.edo * octaves[vi];
        }

        public int distance(Voicing prev)
        {
            int max = 0;

            for(int vi = 0; vi < tetrad.range; vi++)
            {
                int diff = pitch(vi) - prev.pitch(vi);
                if(diff < 0)
                {
                    diff = -diff;
                }
                if(diff > max)
                {
                    max = diff;
                }
            }

            return max;
        }

        public double write(StreamWriter file, double t, double d)
        {
            for (int i = 0; i < tetrad.range; i++)
            {
                tetrad.vectors[order[i]].write(file, t, d * 0.95, 1200, octaves[i]);
            }
            return t + d;
        }

    }
}
