using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class OTetrad : Tetrad
    {
        public OTetrad(Vector v, Tetrad p) : base(v, p)
        {
            for(int i = 1; i < range; i++)
            {
                vectors[i] = new Vector(vectors[0]);
                vectors[i].power[i - 1]++;
            }
        }
        public override Tetrad step(int i, int j)
        {
            Vector root = new Vector(vectors[i]);
            root.power[j - 1] = vectors[j].power[j - 1];
            return new UTetrad(root, this);
        }
        public override Tetrad step(int i)
        {
            Vector root = new Vector(vectors[i]);
             
            return new UTetrad(root, this);
        }

    }
}
