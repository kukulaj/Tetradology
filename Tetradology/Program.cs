using System;

namespace Tetradology
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Lattice TL = new Lattice();
            Vector goal = new Vector();
            goal.power[0] = 7;
            goal.power[1] = -4;
            goal.power[2] = -1;

            Tetrad hit = TL.walk(new OTetrad(goal, null));

            Console.WriteLine("Goodbye World!");
        }
    }
}
