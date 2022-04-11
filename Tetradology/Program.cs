using System;
using System.IO;

namespace Tetradology
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Lattice TL = new Lattice();
            Vector goal = new Vector();
            goal.power[0] = -27;
            goal.power[1] = 18;
            goal.power[2] = 0;
            //goal.power[3] = -1;

            Tetrad hit = TL.walk(new OTetrad(goal, null));

            string vfilename = @"C:\Users\James\Documents\tuning\meantone\vectors.txt";
            StreamWriter vfile = new StreamWriter(vfilename);
            hit.writeVector(vfile);
            vfile.Close();

            string filename = @"C:\Users\James\Documents\tuning\meantone\mtscore.txt";
            StreamWriter file = new StreamWriter(filename);


            file.WriteLine("f1 0 4096 10 1");
            file.WriteLine("f2 0 4096 10 1 0.5 0.3 0.25 0.2 0.167 0.14 0.125 .111");
            file.WriteLine("f3 0 4096 10 1 0.3 0.5 0.25 0.2 0.167 0.14 0.125 .111");
            file.WriteLine("f4 0 4096 10 1 0.3 0.4 0.4");
            file.WriteLine("f5 0 4096 10 1 0.5 0.3");
            file.WriteLine("f6 0 4096 10 1 0.3 0.3 0.3 0.3");
            file.WriteLine("f7 0 4096 10 1 0.6 0.2 0.1 0.3");
            double start = 0.0;
            for(int r = 0; r < 8; r++)
            {
                start = hit.write(file, start, 4);
            }

            file.Close();
            Console.WriteLine("Goodbye World!");
        }
    }
}
