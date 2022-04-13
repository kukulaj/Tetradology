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
        public Loop(Random pr, StreamWriter file, Tetrad init, Tetrad pc)
        {
            rand = pr;
            comma = pc;
            int size = trace(init);

            tetrads = new Tetrad[size];
            
            Tetrad scan = init;
            int ti = 0;
            while (scan.parent != null)
            {
                scan.writeVector(file);
                tetrads[ti] = scan;
                ti++;
                scan = scan.parent;
            }
            file.WriteLine(" ");

            spot = 0;
            t = 0.0;
            d = 4.0;
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


        public int trace(Tetrad start)
        {
            int size = 0;
            Tetrad scan = start;
            while (scan.parent != null)
            {
                size++;
                scan = scan.parent;
            }
            return size;
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
            int gap = 5;
            int starti = 0;
            if (spot > tetrads.Length / 2)
            {
                starti = rand.Next(spot - gap - 1);
            }
            else 
            {
                starti = spot +  1 + rand.Next(tetrads.Length - spot - gap - 2);
            }
           
            int endi = starti + gap;

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


            Tetrad start = tetrads[starti];
            Tetrad endt = tetrads[endi];
            endt.parent = null;
            start.parent = null;

            for(int ti = starti; ti< endi; ti++)
            {
                if(!tetrads[ti].check(tetrads[ti+1]))
                {
                    tetrads[ti+1].subtract(comma);
                    ;
                }
            }

            endt.parent = null;
            start.parent = null;
            Lattice sl = new Lattice(rand);      
            Tetrad hit = sl.walk(endt, start);
            int insert = trace(hit);

           
            
            Tetrad[] replacement = new Tetrad[tetrads.Length - gap + insert];
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
            while(hit != null)
            {
                hit.writeVector(file);
                replacement[newi] = hit;
                newi++;
                hit = hit.parent;
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
