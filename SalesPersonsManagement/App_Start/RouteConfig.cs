using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sales_Persons
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();
/*
            routes.MapRoute(
                "MoviesByReleasedDate",
                "Home/released/{year}/{month}",
                new { Controller = "Home", Action = "ByReleasedDate" },
                new { year=@"\d{4}",month=@"\d{2}"});     
                */             
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
