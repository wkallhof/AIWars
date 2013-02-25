using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWars.Core.Utility;

namespace AIWars.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating Galaxy");
            var galaxyGenerator = new GalaxyGenerator(10, 2, 20);
            var galaxy = galaxyGenerator.Generate();


            var galaxySize = galaxy.Points.Max(x => x.Y);

            for (var y = 1; y <= galaxySize; y++)
            {
                var row = "";
                for (var x = 1; x <= galaxySize; x++)
                {
                    var point = galaxy.Points.Single(p => p.X == x && p.Y == y);
                    row += (point.Populated) ? "*" : "-";
                }

                Console.WriteLine(row);
            }

            Console.Read();

        }
    }
}
