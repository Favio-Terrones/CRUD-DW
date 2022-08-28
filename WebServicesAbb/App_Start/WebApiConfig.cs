using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebServicesAbb.Filters;

namespace WebServicesAbb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MessageHandlers.Add(new FIlterJWT());
        }
    }
}
