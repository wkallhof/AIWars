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
		private readonly int _minimumPlanetSize;
		private readonly int _minimumPlanetDistance;
        private readonly Galaxy _galaxy;

		private bool _keepGrowing;

        public GalaxyGenerator(int maximumPlanetCount, int maximumPlanetSize, int minimumPlanetSize, int galaxyWidth, int galaxyHeight, int minimumPlanetDistance, int galaxySeed = 0)
        {

			if (maximumPlanetCount < 2)
			{
				throw new ArgumentOutOfRangeException("maximumPlanetCount", "The minimum number of planets is two");
			}

            _maximumPlanetCount = maximumPlanetCount;
            _maximumPlanetSize = maximumPlanetSize;
			_minimumPlanetSize = minimumPlanetSize;
			_minimumPlanetDistance = minimumPlanetDistance;
            _randomGenerator = new Random((galaxySeed == 0 ?Environment.TickCount:galaxySeed));
            _galaxy = new Galaxy(galaxyWidth, galaxyHeight);
        }

        /// <summary>
        /// Will generate a galaxy with the provided constructor constraints
        /// </summary>
        /// <returns></returns>
        public Galaxy Generate()
        {
			_galaxy.Planets.Clear();
			PopulateGalaxy();
			_keepGrowing = true;
			while (_keepGrowing)
			{
				GrowPlanets();
			}

			CleanUp();
			PopulatePoints();

            return _galaxy;
        }

        /// <summary>
        /// Pre-populates galaxy with size 1 planets at random locations
        /// </summary>
        private void PopulateGalaxy()
        {

            for (var i = 0; i < _maximumPlanetCount; i++)
            {
                var planet = new Planet(1);
                planet.Point = new Point
                {
                    X = (int)Math.Floor((decimal)(_randomGenerator.Next(_galaxy.Width))),
                    Y = (int)Math.Floor((decimal)(_randomGenerator.Next(_galaxy.Height)))
                };
                _galaxy.Planets.Add(planet);
            }
        }

        /// <summary>
        /// Handles the growing of a galaxies planets and the indicator to stop growing
        /// </summary>
        private void GrowPlanets()
        {
            var growingCount = _galaxy.Planets.Count;
            _galaxy.Planets.ForEach(planet => growingCount = GrowPlanet(planet) ? growingCount : growingCount - 1);
            _keepGrowing = growingCount > 0;
        }

        /// <summary>
        /// Does a forward check to see if a planets growth will break constraints.
        /// If so, do not grow planet
        /// </summary>
        /// <returns>True if planet grew</returns>
        private bool GrowPlanet(Planet planet)
        {
            planet.Size++;

            var planetCollision = _galaxy.Planets.Any(x => CollisionPlanet(x, planet));
            var wallCollision = CollisionWall(planet);
            var planetTooLarge = planet.Size > _maximumPlanetSize;

            var canGrow = !planetTooLarge && !planetCollision && !wallCollision;
            if (!canGrow)
            {
                planet.Size--;
            }

            return canGrow;
        }

        /// <summary>
        /// Will remove all planets that fall below the minimum planet size
        /// </summary>
        private void CleanUp()
        {
            _galaxy.Planets.RemoveAll(x => x.Size < _minimumPlanetSize);
        }

        /// <summary>
        /// For each galaxy point, this checks if the point is populated by a planet, if so,
        /// set the populated flag on the point
        /// </summary>
		private void PopulatePoints()
		{
			_galaxy.Points.ForEach(point => point.Populated = _galaxy.Planets.Any(planet => IsPointInCircle(point, planet.Point, planet.Size)));
		}

        /// <summary>
        /// Determine if a given planet is outside of the galaxy walls
        /// </summary>
        /// <returns>True if the planet is outside of the constraints</returns>
		private bool CollisionWall(Planet planet)
		{
			var topX = planet.Point.X - planet.Size;
			var topY = planet.Point.Y - planet.Size;
			var bottomX = planet.Point.X + planet.Size;
			var bottomY = planet.Point.Y + planet.Size;

			var widthCheck = ((topX > 0) && (bottomX < _galaxy.Width));
			var heightCheck = ((topY > 0) && (bottomY < _galaxy.Height));
	
			return !(widthCheck && heightCheck);
		}

        /// <summary>
        /// Checks if the given planets fall within "collision" range of each other
        /// (this includes the minimum planet distance)
        /// </summary>
        /// <returns>True if the two planets are too close</returns>
		private bool CollisionPlanet(Planet planet1, Planet planet2)
		{
            if (planet1 == planet2) return false;
			if (Math.Abs(planet2.Point.X - planet1.Point.X) > (planet2.Size + planet1.Size + _minimumPlanetDistance)) return false;
			if (Math.Abs(planet2.Point.Y - planet1.Point.Y) > (planet2.Size + planet1.Size + _minimumPlanetDistance)) return false;

			var distance = GetDistance(planet1.Point, planet2.Point);
			return !(distance > (planet1.Size + planet2.Size + _minimumPlanetDistance));
		}

        #region Math Helpers
        /// <summary>
        /// Checks the distance between two points
        /// </summary>
        private double GetDistance(Point point1, Point point2)
        {
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);
            return Math.Sqrt(a * a + b * b);
        }

        /// <summary>
        /// Checks if a given point falls within a given circle
        /// </summary>
        private bool IsPointInCircle(Point point, Point circlePoint, int circleR)
        {
            var distance = GetDistance(point, circlePoint);
            return distance <= circleR;
        }
        #endregion

    }
}
