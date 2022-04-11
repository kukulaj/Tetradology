using System;
using System.Collections.Generic;
using System.Text;

namespace Tetradology
{
    public class Fuzz
    {
        const int fcnt = 20;
        double[] phase;
        double power;
        Random rand;
        public Fuzz(Random pr, double ppower)
        {
            rand = pr;
            power = ppower;
            phase = new double[fcnt];
            {
                for(int i = 0; i < fcnt; i++)
                {
                    phase[i] = rand.NextDouble();
                }
            }
        }

        public double noise(double t)
        {
            double result = 1.0;

            double factor = 0.02;
            double wlength = 1.0;
            for(int i = 0; i < fcnt; i++)
            {
                result += factor * Math.Sin(Math.PI * 2.0 * phase[i]
                    + t / wlength);
                wlength *= 1.3;
                factor *= power;
            }

            return result;
        }

    }
}
