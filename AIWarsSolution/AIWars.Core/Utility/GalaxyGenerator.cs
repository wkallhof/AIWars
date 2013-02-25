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

        public GalaxyGenerator(int maximumPlanetCount, int maximumPlanetSize, int minimumPlanetSize, int galaxySize, int minimumPlanetDistance, int galaxySeed = 0)
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
            _galaxy = new Galaxy(galaxySize);
        }


		private bool IsPointInCircle(Point point, Point circlePoint, int circleR)
		{
			var xs = 0;
			var ys = 0;
			xs = circlePoint.X - point.X;
			xs = xs * xs;
			ys = circlePoint.Y - point.Y;
			ys = ys * ys;

			var distance = (int)Math.Floor((decimal)Math.Sqrt(xs + ys));

			return distance <= circleR;

		}

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

		private void PopulatePoints()
		{
			_galaxy.Points.ForEach(point => point.Populated = _galaxy.Planets.Any(planet => IsPointInCircle(point, planet.Point, planet.Size)));
		}

		private bool CollisionWall(Planet planet)
		{
			var galaxySize = (int)Math.Round(Math.Sqrt(_galaxy.Points.Count));

			var topX = planet.Point.X - planet.Size;
			var topY = planet.Point.Y - planet.Size;
			var bottomX = planet.Point.X + planet.Size;
			var bottomY = planet.Point.Y + planet.Size;

			var widthCheck = ((topX > 0) && (bottomX < galaxySize));
			var heightCheck = ((topY > 0) && (bottomY < galaxySize));
	
			return !(widthCheck && heightCheck);
		}

		private bool CollisionPlanet(Planet planet1, Planet planet2)
		{
			if (Math.Abs(planet2.Point.X - planet1.Point.X) > (planet2.Size + planet1.Size + _minimumPlanetDistance)) return false;
			if (Math.Abs(planet2.Point.Y - planet1.Point.Y) > (planet2.Size + planet1.Size + _minimumPlanetDistance)) return false;

			var distance = GetDistance(planet1, planet2);
			return !(distance > (planet1.Size + planet2.Size + _minimumPlanetDistance));
		}

		private int GetDistance(Planet planet1, Planet planet2)
		{
			var xs = 0;
			var ys = 0;
			xs = planet2.Point.X - planet1.Point.X;
			xs = xs * xs;
			ys = planet2.Point.Y - planet1.Point.Y;
			ys = ys * ys;
			return (int)Math.Floor((decimal)Math.Sqrt(xs + ys));
		}

		private void PopulateGalaxy()
		{
			var galaxySize =  (int)Math.Round(Math.Sqrt(_galaxy.Points.Count));

			for(var i = 0; i < _maximumPlanetCount; i++)
			{

				var randomX = (int)Math.Floor((decimal)(_randomGenerator.Next(galaxySize)));
				var randomY = (int)Math.Floor((decimal)(_randomGenerator.Next(galaxySize)));
				var planet = new Planet(1);
				planet.Point = new Point { X = randomX, Y = randomY };
				_galaxy.Planets.Add(planet);
			}
		}

		private void ClearGalaxy()
		{
			_galaxy.Planets.Clear();
		}

		private bool GrowPlanet(Planet planet)
		{
			var canGrow = planet.Size <= _maximumPlanetSize;
			for (var index = 0; index < _galaxy.Planets.Count; index++)
			{
				if (_galaxy.Planets[index] != planet)
				{
					canGrow = canGrow && !CollisionPlanet(_galaxy.Planets[index],planet) && !CollisionWall(planet);
				}
			}
    
			if(canGrow)
			{
				planet.Size++;
			}

			return canGrow;
		}

		private void GrowPlanets()
		{
			var growingCount = _galaxy.Planets.Count;
			for(var index = 0; index < _galaxy.Planets.Count; index++)
			{
				var growth = GrowPlanet(_galaxy.Planets[index]);
				if(!growth) growingCount--;
			
			}
			_keepGrowing = growingCount > 0;
		}

		private void CleanUp()
		{
			for (var index = 0; index < _galaxy.Planets.Count; index++)
			{
				if (_galaxy.Planets[index].Size <= _minimumPlanetSize)
				{
					_galaxy.Planets.RemoveAt(index);
				}

			}
		}
    }
}
