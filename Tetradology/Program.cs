using System;
using System.IO;

namespace Tetradology
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Random rand = new Random(110);

            Lattice TL = new Lattice(rand);
            Vector goal = new Vector();
            goal.power[0] = 1;
            goal.power[1] = 2;
            goal.power[2] = -4;
            //goal.power[3] = 1;


            Tetrad comma = new OTetrad(goal);
            Tetrad[] path = TL.walk(new OTetrad(new Vector()), comma);

            string vfilename = @"C:\Users\James\Documents\tuning\meantone\vectors.txt";
            StreamWriter vfile = new StreamWriter(vfilename);
            
           

            string filename = @"C:\Users\James\Documents\tuning\meantone\mtscore.txt";
            StreamWriter file = new StreamWriter(filename);


            file.WriteLine("f1 0 4096 10 1");
            file.WriteLine("f2 0 4096 10 1 0.5 0.3 0.25 0.2 0.167 0.14 0.125 .111");
            file.WriteLine("f3 0 4096 10 1 0.3 0.5 0.25 0.2 0.167 0.14 0.125 .111");
            file.WriteLine("f4 0 4096 10 1 0.3 0.4 0.4");
            file.WriteLine("f5 0 4096 10 1 0.5 0.3");
            file.WriteLine("f6 0 4096 10 1 0.3 0.3 0.3 0.3");
            file.WriteLine("f7 0 4096 10 1 0.6 0.2 0.1 0.3");

            Loop loop = new Loop(rand, vfile, path, comma);
            for (int si = 0; si < 50; si++)
            {
                loop.write(file, 6 + rand.Next(6));
                loop.swap(vfile);
            }

            loop.writeVectors(vfile);
            file.Close();
            vfile.Close();
            Console.WriteLine("Goodbye World!");
        }
    }
}
