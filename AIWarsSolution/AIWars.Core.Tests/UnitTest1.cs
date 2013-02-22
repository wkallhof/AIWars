using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace AIWars.Core.Tests
{
    [TestClass]
    public class MapGeneratorTests
    {
        //Generate returns a map object
        [TestMethod]
        public void Generate_returns_a_map_object()
        {
            //Arrage
            var g = new MapGenerator(10, 2, 50);

            //Act
            var result = g.Generate();

            //Assert
            Assert.IsInstanceOfType(result, typeof(Map));
        }

        //Generated node count does not exceed the maximum node count requested
        [TestMethod]
        public void Generated_node_count_does_not_exceed_the_maximum_node_count_requested()
        {
            //Arrange
            var g = new MapGenerator(10, 2, 50);

            //Act
            var result = g.Generate();

            //Assert
            Assert.IsNotNull(result.Nodes);
            Assert.IsTrue(result.Nodes.Count() > 0);
        }

        //The number of nodes requested can not be less than two
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum number of nodes is two")]
        public void The_number_of_nodes_requested_can_not_be_less_than_two()
        {
            //Arrange
            var g = new MapGenerator(1, 2, 50);

            //Act
            var result = g.Generate();
        }

        //No nodes generated can be greater than the max node size requested
        [TestMethod]
        public void No_nodes_generated_can_be_greater_than_the_max_node_size_requested()
        {
            //Arrange
            var g = new MapGenerator(10, 2, 50);

            //Act
            var map = g.Generate();

            //Assert
            Assert.IsFalse(map.Nodes.Any(x => x.Size > 2));
        }

        [TestMethod]
        public void No_nodes_can_be_sized_less_than_one()
        {
            //Arrange
            var g = new MapGenerator(10, 2, 50);

            //Act
            var map = g.Generate();

            //Assert
            Assert.IsFalse(map.Nodes.Any(x => x.Size < 1));
        }

        //The generated map matches the map size requested
        [TestMethod]
        public void The_generated_map_matches_the_map_size_requested()
        {
            //Arrange
            var g = new MapGenerator(10, 2, 50);

            //Act
            var map = g.Generate();

            //Assert
            Assert.IsFalse(map.Points.Any(x => x.X > 50));
            Assert.IsFalse(map.Points.Any(x => x.Y > 50));
        }

        //The map can not be smaller than four times the max node size requested
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The minimum map size for size 3 nodes is 12")]
        public void The_map_can_not_be_smaller_than_four_times_the_max_node_size_requested()
        {
            //Arrange
            var g = new MapGenerator(10, 3, 8);

            //Act
            var result = g.Generate();
        }


        //No nodes can overlap
        //The maximum new node size property returns the correct value
        //The maximum new node size property can not exceed the max node size requested
    }

    public class MapGenerator
    {
        private readonly Random _randomGenerator;
        private readonly int _maximumNodeCount;
        private readonly int _maximumNodeSize;
        private readonly Map _map;

        public MapGenerator(int maximumNodeCount, int maximumNodeSize, int mapSize)
        {
            if (maximumNodeCount < 2)
            {
                throw new ArgumentOutOfRangeException("maximumNodeCount", "The minimum number of nodes is two");
            }

            if (mapSize < (maximumNodeSize * 4))
            {
                var exceptionMsg = string.Format("The minimum map size for size {0} nodes is {1}", maximumNodeSize, maximumNodeSize * 4);
                throw new ArgumentOutOfRangeException("mapSize", exceptionMsg);
            }

            _maximumNodeCount = maximumNodeCount;
            _maximumNodeSize = maximumNodeSize;
            _map = new Map(mapSize);
            _randomGenerator = new Random(Environment.TickCount);
        }

        public Map Generate()
        {
            for (int i = 1; i <= _maximumNodeCount; i++)
            {
                AddNode(i);
            }

            return _map;
        }

        private void AddNode(int nodeIndex)
        {
            var node = new Node(_randomGenerator.Next(1, _maximumNodeSize + 1));
            _map.Nodes.Add(node);
        }
    }

    public class Map
    {
        private List<Node> _nodes;
        private List<Point> _points;

        public Map(int mapSize)
        {
            _nodes = new List<Node>();
            _points = new List<Point>();

            PopulatePoints(mapSize);
        }

        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public List<Point> Points
        {
            get
            {
                return _points;
            }
        }

        private void PopulatePoints(int mapSize)
        {
            for (int x = 1; x <= mapSize; x++)
            {
                for (int y = 1; y <= mapSize; y++)
                {
                    _points.Add(new Point { Populated = false, X = x, Y = y });
                }
            }
        }

    }

    public class Node
    {
        private readonly int _size;

        public Node(int size)
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
