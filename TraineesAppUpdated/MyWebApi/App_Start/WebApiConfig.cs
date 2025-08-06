using Newtonsoft.Json;
using System.Web.Http;

namespace MyWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();
            // Web API configuration and services
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.TypeNameHandling = TypeNameHandling.None;

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
