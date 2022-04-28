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
            
            for(int ti = 0; ti<size; ti++)
            {
                path[ti+1].writeVector(file);
                tetrads[ti] = path[ti+1];
            }
            file.WriteLine(" ");

            spot = 0;
            t = 0.0;
            d = 0.6;
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
                 t = tetrads[spot].write(file, t, d);
                spot = (spot + 1) % tetrads.Length;
                
            }

        }

        public void swap(StreamWriter file)
        {
            int gap =7;
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

            Tetrad[] replacement = new Tetrad[tetrads.Length - gap + insert-1];
            Console.WriteLine(string.Format("new loop is {0} long", replacement.Length));

            int oldi = spot;
            int newi = 0;

            while(oldi != starti)
            {
                replacement[newi] = tetrads[oldi];
                newi++;
                oldi = (oldi + 1) % tetrads.Length;
            }

            file.WriteLine("to:");
            for(int pi = 0; pi< path.Length; pi++)
            {
                path[pi].writeVector(file);
                replacement[newi] = path[pi];
                newi++;
            }

            oldi = (endi + 1) % tetrads.Length;

            while (oldi != spot)
            {
                replacement[newi] = tetrads[oldi];
                newi++;
                oldi = (oldi + 1) % tetrads.Length;
            }
            Debug.Assert(newi == replacement.Length);


            tetrads = replacement;
            spot = 0;
        }


    }
}
