using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Core.Model
{
    public class Galaxy
    {
        private readonly List<Planet> _planets;
        private readonly List<Point> _points;

        public Galaxy(int galaxySize)
        {
            _planets = new List<Planet>();
            _points = new List<Point>();

            PopulatePoints(galaxySize);
        }

        public List<Planet> Planets
        {
            get
            {
                return _planets;
            }
        }

        public List<Point> Points
        {
            get
            {
                return _points;
            }
        }

        private void PopulatePoints(int galaxySize)
        {
            for (int x = 1; x <= galaxySize; x++)
            {
                for (int y = 1; y <= galaxySize; y++)
                {
                    _points.Add(new Point { Populated = false, X = x, Y = y });
                }
            }
        }
    }
}
