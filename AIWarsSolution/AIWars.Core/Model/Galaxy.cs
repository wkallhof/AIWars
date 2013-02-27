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
        public readonly int Height;
        public readonly int Width;

        public Galaxy(int galaxyWidth, int galaxyHeight)
        {
            _planets = new List<Planet>();
            _points = new List<Point>();

            Height = galaxyHeight;
            Width = galaxyWidth;

            PopulatePoints(galaxyWidth, galaxyHeight);
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

        private void PopulatePoints(int galaxyWidth, int galaxyHeight)
        {
            for (int x = 0; x <= galaxyWidth; x++)
            {
                for (int y = 0; y <= galaxyHeight; y++)
                {
                    _points.Add(new Point { Populated = false, X = x, Y = y });
                }
            }
        }
    }
}
