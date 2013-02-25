using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWars.Core.Utility;
using AIWars.Core.Model;

namespace AIWars.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating Galaxy");
			//var galaxyGenerator = new GalaxyGenerator(10, 4, 1, 50, 2);
			var galaxyGenerator = new GalaxyGenerator(50, 10, 4, 75, 5);
            var galaxy = galaxyGenerator.Generate();


            var galaxySize = galaxy.Points.Max(x => x.Y);

            for (var y = 0; y <= galaxySize; y++)
            {
                var row = "";
                for (var x = 0; x <= galaxySize; x++)
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
