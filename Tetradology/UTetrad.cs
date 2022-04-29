using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class UTetrad : Tetrad
    {
        public UTetrad(Vector v) : base(v)
        {
            for (int i = 1; i < range; i++)
            {
                vectors[i] = new Vector(vectors[0]);
                vectors[i].power[i - 1]--;
            }
        }

        public override int shift(int i)
        {
            return i/2;
        }
        public override Tetrad subtract(Tetrad t2)
        {
            Vector root = new Vector(vectors[0]);
            root.subtract(t2.vectors[0]);
            return new UTetrad(root);
        }

        public override Tetrad step(int i)
        {
            Vector root = new Vector(vectors[i]);
             
            return new OTetrad(root);
        }

        public override Tetrad step(int i, int j)
        {
            Vector root = new Vector(vectors[i]);
            root.power[j - 1] = vectors[j].power[j - 1];
            return new OTetrad(root);
        }
    }
}
