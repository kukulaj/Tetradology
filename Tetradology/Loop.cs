using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Tetradology
{
    public class Loop
    {
        Random rand;
        Tetrad[] tetrads;
        Voicing[] voicings;
        int spot;
        double t;
        double d;
        Tetrad comma;
         
        public Loop(Random pr, StreamWriter file, Tetrad[]path, Tetrad pc)
        {
            rand = pr;
            comma = pc;
            int size = path.Length-1;
           

            tetrads = new Tetrad[size];
            voicings = new Voicing[size];
            
            for(int ti = 0; ti<size; ti++)
            {
                path[ti+1].writeVector(file);
                tetrads[ti] = path[ti+1];
                if (ti == 0)
                {
                    voicings[0] = new Voicing(tetrads[0]);
                }
                else
                {
                    VoicingFactory vf = new VoicingFactory(tetrads[ti], voicings[ti-1]);
                    voicings[ti] = vf.getBest();
                }
            }


            for (int ti = 0; ti < 4 * size; ti++)
            {
                int previ = (ti + size - 1) % size;
                int tweaki = ti  % size;
                int nexti = (ti + 1) % size;

                VoicingFactory vf = new VoicingFactory(tetrads[tweaki],
                    voicings[previ], voicings[nexti]);
                voicings[tweaki] = vf.getBest();
            }

            file.WriteLine(" ");

            spot = 0;
            t = 0.0;
            d = 1.6;
        }

        public void writeVectors(StreamWriter file)
        {
            file.WriteLine("loop:");
            for(int i = 0; i < tetrads.Length; i++)
            {
                tetrads[i].writeVector(file);
            }
            file.WriteLine(" ");
        }

        public void write(StreamWriter file, int cnt)
        {
            for(int ci=0; ci < cnt; ci++)
            {
                 t = voicings[spot].write(file, t, d);
                spot = (spot + 1) % tetrads.Length;
            }

        }

        public void swap(StreamWriter file)
        {
            int gap =4;
            int starti = 0;
            
            starti = (spot + rand.Next(tetrads.Length - gap - 2)) % tetrads.Length;
            int endi = (starti + gap) % tetrads.Length;

            file.WriteLine("from");
            int wi = starti;
            bool wdone = false;
            while(!wdone)
            {
                tetrads[wi].writeVector(file);
                if(wi == endi)
                {
                    wdone = true;
                }
                wi = (wi + 1) % tetrads.Length;
            }

            bool leap = false;
            int ti = starti;
            bool ldone = false;
            while(!ldone)
            {
                int nexti = (ti + 1) % tetrads.Length;
                if(!tetrads[ti].check(tetrads[nexti]))
                {
                    leap = true;
                }
                if(nexti == endi)
                {
                    ldone = true;
                }
                else 
                {
                    ti = nexti;
                }
            }
            Tetrad start = tetrads[starti];
            Tetrad endt = tetrads[endi];
            if(leap)
            {
                start = start.subtract(comma);
            }
            
            Lattice sl = new Lattice(rand);      
            Tetrad[] path = sl.walk(start, endt);
            int insert = path.Length;
            Debug.Assert(insert == gap+1);

            bool okay = start.equals(path[0]);
            Debug.Assert(okay   );
            okay = endt.equals(path[insert-1]);
            Debug.Assert(okay);

            int newlen = tetrads.Length - gap + insert - 1;
            Tetrad[] replacement = new Tetrad[newlen];
            Voicing[] vrep = new Voicing[newlen];
            Console.WriteLine(string.Format("new loop is {0} long", replacement.Length));

            int oldi = spot;
            int newi = 0;

            while(oldi != starti)
            {
                replacement[newi] = tetrads[oldi];
                vrep[newi] = voicings[oldi];
                newi++;
                oldi = (oldi + 1) % tetrads.Length;
            }

            file.WriteLine("to:");
            int firstnew = newi;
            for(int pi = 0; pi< path.Length; pi++)
            {
                path[pi].writeVector(file);
                replacement[newi] = path[pi];
                VoicingFactory vf;
                if (pi == 0)
                {
                    vf = new VoicingFactory(path[pi],
                        voicings[(tetrads.Length + oldi - 1) % tetrads.Length]) ;
                }
                else 
                {
                    vf = new VoicingFactory(path[pi], vrep[newi - 1]);
                }
                 
                vrep[newi] = vf.getBest();
                newi++;
            }
            int lastnew = newi - 1;

            oldi = (endi + 1) % tetrads.Length;

            while (oldi != spot)
            {
                replacement[newi] = tetrads[oldi];
                vrep[newi] = voicings[oldi];
                newi++;
                oldi = (oldi + 1) % tetrads.Length;
            }
            Debug.Assert(newi == replacement.Length);

            for (int ri = 0; ri < 4; ri++)
            {
                for (int twi = 0; twi < gap; twi++)
                {
                    int previ = (vrep.Length +firstnew + twi - 1) % vrep.Length;
                    int tweaki = (previ + 1) % vrep.Length;
                    int nexti = (tweaki + 1) % vrep.Length;

                    VoicingFactory vf = new VoicingFactory(replacement[tweaki],
                        vrep[previ], vrep[nexti]);
                    vrep[tweaki] = vf.getBest();
                }
            }


            tetrads = replacement;
            voicings = vrep;
            spot = 0;
        }


    }
}
