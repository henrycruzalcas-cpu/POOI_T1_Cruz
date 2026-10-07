using System.Web.Mvc;
using System.Web.Routing;

namespace POOI_T2_CruzAlcas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{dni}",
                new { controller = "Alumno", action = "Index", dni = UrlParameter.Optional });
        }
    }
}
