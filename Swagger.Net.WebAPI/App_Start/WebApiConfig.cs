using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Swagger.Net.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                name: "Custom route for linking a pet to a user",
                routeTemplate: "api/Pet/{id}/User/{userId}",
                defaults: new { controller="Pet", action="PutUser", userId = RouteParameter.Optional },
                constraints: new { id = @"\d+", userId = @"\d*" });

            configuration.Routes.MapHttpRoute(
                name: "Controller Actions That Are Not HTTP Verbs",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { action = @"\D+", id = @"\d*", });

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
