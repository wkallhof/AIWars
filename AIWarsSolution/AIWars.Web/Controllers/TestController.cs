using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIWars.Core.Utility;
using AIWars.Web.Models;

namespace AIWars.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Galaxy(GenerateGalaxyRequest request)
        {
            var galaxyGenerator = new GalaxyGenerator(request.MaximumPlanetCount, request.MaximumPlanetSize,
                                                      request.MinimumPlanetSize, request.GalaxySize,
                                                      request.MinimumPlanetSize);
            var galaxy = galaxyGenerator.Generate();

            return View(galaxy);
        }

    }
}
