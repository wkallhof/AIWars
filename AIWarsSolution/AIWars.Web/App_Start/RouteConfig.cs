﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AIWars.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //var galaxyGenerator = new GalaxyGenerator(50, 10, 4, 75, 5);
            //int maximumPlanetCount, int maximumPlanetSize, int minimumPlanetSize, int galaxySize, int minimumPlanetDistance

            routes.MapRoute(
                "GenerateGalaxyRoute",
                "Test/Galaxy/{galaxySize}/{maximumPlanetCount}/{maximumPlanetSize}/{minimumPlanetSize}/{minimumPlanetDistance}",
                new { controller = "Test", action = "Galaxy", galaxySize = 75, maximumPlanetCount = 50, maximumPlanetSize = 10, minimumPlanetSize = 4, minimumPlanetDistance = 5 }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}