using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWars.Core.Model;

namespace AIWars.Core.Utility
{
    public class GalaxyGenerator
    {
        private readonly Random _randomGenerator;
        private readonly int _maximumPlanetCount;
        private readonly int _maximumPlanetSize;
        private readonly Galaxy _galaxy;

        public GalaxyGenerator(int maximumPlanetCount, int maximumPlanetSize, int galaxySize)
        {
            if (maximumPlanetCount < 2)
            {
                throw new ArgumentOutOfRangeException("maximumPlanetCount", "The minimum number of planets is two");
            }

            if (galaxySize < (maximumPlanetSize * 4))
            {
                var exceptionMsg = string.Format("The minimum galaxy size for size {0} planets is {1}", maximumPlanetSize, maximumPlanetSize * 4);
                throw new ArgumentOutOfRangeException("galaxySize", exceptionMsg);
            }

            _maximumPlanetCount = maximumPlanetCount;
            _maximumPlanetSize = maximumPlanetSize;
            _randomGenerator = new Random(Environment.TickCount);
            _galaxy = new Galaxy(galaxySize);
        }

        public Galaxy Generate()
        {
            for (var i = 1; i <= _maximumPlanetCount; i++)
            {
                AddPlanet();
            }

            return _galaxy;
        }

        private void AddPlanet()
        {
            var currentMaxPlanetSize = DetermineMaxPlanetSize();
            if (currentMaxPlanetSize < 1)
            {
                return;
            }

            currentMaxPlanetSize = (currentMaxPlanetSize < _maximumPlanetSize) ? currentMaxPlanetSize : _maximumPlanetSize;

            var planet = new Planet(_randomGenerator.Next(1, currentMaxPlanetSize + 1));

            var availablePoints = AvailablePointsForPlanet(planet);
            var selectedPointIndex = _randomGenerator.Next(0, availablePoints.Count());

            planet.Point = availablePoints[selectedPointIndex];
            planet.Point.Populated = true;
            PopulatePoints(planet.Point, planet.Size);
            _galaxy.Planets.Add(planet);
        }

        private void PopulatePoints(Point point, int size)
        {
            for (int i = 1; i <= size; i++)
            {
                var north = _galaxy.Points.SingleOrDefault(northPoint => northPoint.X == point.X && northPoint.Y == point.Y + i);
                var south = _galaxy.Points.SingleOrDefault(southPoint => southPoint.X == point.X && southPoint.Y == point.Y - 1);
                var east = _galaxy.Points.SingleOrDefault(eastPoint => eastPoint.X == point.X + i && eastPoint.Y == point.Y);
                var west = _galaxy.Points.SingleOrDefault(westPoint => westPoint.X == point.X - i && westPoint.Y == point.Y);

                if (north != null)
                    north.Populated = true;
                if (south != null)
                    south.Populated = true;
                if (east != null)
                    east.Populated = true;
                if (west != null)
                    west.Populated = true;
            }
        }

        private List<Point> AvailablePointsForPlanet(Planet planet)
        {
            var availablePoints = new List<Point>();
            foreach (var point in _galaxy.Points.Where(x => x.Populated == false))
            {
                var distances = FetchPointDistances(point);
                if (distances.Min() >= planet.Size)
                {
                    availablePoints.Add(point);
                }
            }

            return availablePoints;
        }

        private int DetermineMaxPlanetSize()
        {
            var maxPlanetSize = 0;

            Parallel.ForEach(_galaxy.Points, point =>
                {
                    var distances = FetchPointDistances(point);
                    var minimum = distances.Min();

                    if (maxPlanetSize < minimum)
                    {
                        maxPlanetSize = minimum;
                    }
                });

            return maxPlanetSize;
        }

        public IEnumerable<int> FetchPointDistances(Point point)
        {
            var distances = new List<int>
                    {
                        OpenDistance(point, Direction.North),
                        OpenDistance(point, Direction.South),
                        OpenDistance(point, Direction.East),
                        OpenDistance(point, Direction.West)
                    };
            return distances;
        }

        private int OpenDistance(Point point, Direction direction)
        {
            var distance = 0;
            var mapSize = _galaxy.Points.Max(x => x.X);
            var pointTuple = new Tuple<int, int>(point.X, point.Y);

            Func<int, int, Tuple<int, int>> adjustCoordinates;

            switch (direction)
            {
                case Direction.North:
                    adjustCoordinates = (x, y) => { y--; return new Tuple<int, int>(x, y); };
                    break;
                case Direction.South:
                    adjustCoordinates = (x, y) => { y++; return new Tuple<int, int>(x, y); };
                    break;
                case Direction.East:
                    adjustCoordinates = (x, y) => { x++; return new Tuple<int, int>(x, y); };
                    break;
                case Direction.West:
                    adjustCoordinates = (x, y) => { x--; return new Tuple<int, int>(x, y); };
                    break;
                default:
                    throw new ArgumentException("A direction is required before calculating distance");
            }

            while (pointTuple.Item1 >= 1 && pointTuple.Item2 >= 1 && pointTuple.Item1 < mapSize && pointTuple.Item2 < mapSize)
            {
                pointTuple = adjustCoordinates(pointTuple.Item1, pointTuple.Item2);
                var existingPoint =
                    _galaxy.Points.SingleOrDefault(
                        tempPoint => tempPoint.X == pointTuple.Item1 && tempPoint.Y == pointTuple.Item2);
                if (existingPoint != null && !existingPoint.Populated)
                {
                    distance++;
                }
            }

            return distance;
        }
    }
}
