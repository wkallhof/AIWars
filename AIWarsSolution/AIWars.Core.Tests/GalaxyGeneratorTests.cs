using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace AIWars.Core.Tests
{
    [TestClass]
    public class GalaxyGeneratorTests
    {
        //Generate returns a galaxy object
        [TestMethod]
        public void Generate_returns_a_galaxy_object()
        {
            //Arrage
            var g = new GalaxyGenerator(10, 2, 50);

            //Act
            var result = g.Generate();

            //Assert
            Assert.IsInstanceOfType(result, typeof(galaxy));
        }

        //Generated planet count does not exceed the maximum planet count requested
        [TestMethod]
        public void Generated_planet_count_does_not_exceed_the_maximum_planet_count_requested()
        {
            //Arrange
            var g = new GalaxyGenerator(10, 2, 50);

            //Act
            var result = g.Generate();

            //Assert
            Assert.IsNotNull(result.Planets);
            Assert.IsTrue(result.Planets.Count() > 0);
        }

        //The number of planets requested can not be less than two
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum number of planets is two")]
        public void The_number_of_planets_requested_can_not_be_less_than_two()
        {
            //Arrange
            var g = new GalaxyGenerator(1, 2, 50);

            //Act
            var result = g.Generate();
        }

        //No planets generated can be greater than the max planet size requested
        [TestMethod]
        public void No_planets_generated_can_be_greater_than_the_max_planet_size_requested()
        {
            //Arrange
            var g = new GalaxyGenerator(10, 2, 50);

            //Act
            var galaxy = g.Generate();

            //Assert
            Assert.IsFalse(galaxy.Planets.Any(x => x.Size > 2));
        }

        [TestMethod]
        public void No_planets_can_be_sized_less_than_one()
        {
            //Arrange
            var g = new GalaxyGenerator(10, 2, 50);

            //Act
            var galaxy = g.Generate();

            //Assert
            Assert.IsFalse(galaxy.Planets.Any(x => x.Size < 1));
        }

        //The generated galaxy matches the galaxy size requested
        [TestMethod]
        public void The_generated_galaxy_matches_the_galaxy_size_requested()
        {
            //Arrange
            var g = new GalaxyGenerator(10, 2, 50);

            //Act
            var galaxy = g.Generate();

            //Assert
            Assert.IsFalse(galaxy.Points.Any(x => x.X > 50));
            Assert.IsFalse(galaxy.Points.Any(x => x.Y > 50));
        }

        //The galaxy can not be smaller than four times the max planet size requested
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum galaxy size for size 3 planets is 12")]
        public void The_galaxy_can_not_be_smaller_than_four_times_the_max_planet_size_requested()
        {
            //Arrange
            var g = new GalaxyGenerator(10, 3, 8);

            //Act
            var result = g.Generate();
        }


        //No planets can overlap
        //The maximum new planet size property returns the correct value
        //The maximum new planet size property can not exceed the max planet size requested
    }

    public class GalaxyGenerator
    {
        private readonly Random _randomGenerator;
        private readonly int _maximumPlanetCount;
        private readonly int _maximumPlanetSize;
        private readonly galaxy _galaxy;

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
            _galaxy = new galaxy(galaxySize);
            _randomGenerator = new Random(Environment.TickCount);
        }

        public galaxy Generate()
        {
            for (int i = 1; i <= _maximumPlanetCount; i++)
            {
                AddPlanet(i);
            }

            return _galaxy;
        }

        private void AddPlanet(int planetIndex)
        {
            var planet = new Planet(_randomGenerator.Next(1, _maximumPlanetSize + 1));
            _galaxy.Planets.Add(planet);
        }
    }

    public class galaxy
    {
        private List<Planet> _planets;
        private List<Point> _points;

        public galaxy(int galaxySize)
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

    public class Planet
    {
        private readonly int _size;

        public Planet(int size)
        {
            _size = size;
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }
   
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Populated { get; set; }
    }
}
