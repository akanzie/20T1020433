using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _20T1020433.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, },
                new[] { "_20T1020433.Web.Controllers" }
            );
            routes.MapRoute(
            name: "NotFound",
            url: "Error/NotFound",
            defaults: new { controller = "Error", action = "NotFound" }
            );
            routes.MapRoute(
                name: "ServerError",
                url: "Error/ServerError",
                defaults: new { controller = "Error", action = "ServerError" }
            );
        }
    }
}
