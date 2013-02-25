using System;
using System.Linq;
using AIWars.Core.Model;
using AIWars.Core.Utility;
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
			var g = new GalaxyGenerator(50, 10, 4, 75, 5);

			//Act
			var result = g.Generate();

			//Assert
			Assert.IsInstanceOfType(result, typeof(Galaxy));
		}

		//Generated planet count does not exceed the maximum planet count requested
		[TestMethod]
		public void Generated_planet_count_does_not_exceed_the_maximum_planet_count_requested()
		{
			//Arrange
			var g = new GalaxyGenerator(2, 10, 4, 75, 5);

			//Act
			var result = g.Generate();

			//Assert
			Assert.IsNotNull(result.Planets);
			Assert.IsTrue(result.Planets.Count() <= 2);
		}

		//The number of planets requested can not be less than two
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum number of planets is two")]
		public void The_number_of_planets_requested_can_not_be_less_than_two()
		{
			//Arrange
			var g = new GalaxyGenerator(1, 10, 4, 75, 5);

			//Act
			var result = g.Generate();
		}

		//No planets generated can be greater than the max planet size requested
		[TestMethod]
		public void No_planets_generated_can_be_greater_than_the_max_planet_size_requested()
		{
			//Arrange
			var g = new GalaxyGenerator(50, 5, 1, 75, 5);

			//Act
			var galaxy = g.Generate();

			//Assert
			Assert.IsFalse(galaxy.Planets.Any(x => x.Size >= 5));
		}

		[TestMethod]
		public void No_planets_generated_can_be_less_than_the_min_planet_size_requested()
		{
			//Arrange
			var g = new GalaxyGenerator(50, 10, 4, 75, 5);

			//Act
			var galaxy = g.Generate();

			//Assert
			Assert.IsFalse(galaxy.Planets.Any(x => x.Size <= 4));
		}

		//The generated galaxy matches the galaxy size requested
		[TestMethod]
		public void The_generated_galaxy_matches_the_galaxy_size_requested()
		{
			//Arrange
			var g = new GalaxyGenerator(50, 10, 4, 50, 5);

			//Act
			var galaxy = g.Generate();

			//Assert
			Assert.IsFalse(galaxy.Points.Any(x => x.X > 50));
			Assert.IsFalse(galaxy.Points.Any(x => x.Y > 50));
		}


        //No planets can overlap
        //The maximum new planet size property returns the correct value
        //The maximum new planet size property can not exceed the max planet size requested
    }
}
