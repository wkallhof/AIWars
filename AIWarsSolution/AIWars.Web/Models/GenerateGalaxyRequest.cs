﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIWars.Web.Models
{
    public class GenerateGalaxyRequest
    {
        public int GalaxySize { get; set; }
        public int MaximumPlanetCount { get; set; }
        public int MaximumPlanetSize { get; set; }
        public int MinimumPlanetSize { get; set; }
        public int MinimumPlanetDistance { get; set; }
    }
}