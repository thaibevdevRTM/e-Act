using System.Web.Mvc;
using System.Web.Routing;

namespace eActForm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "tbwebbackend",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DashBoard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
