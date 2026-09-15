using RestaurantManagement.Handlers;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace RestaurantManagement
{

    /// <summary>
    /// Configures routing and response formatting for the Web API application.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers attribute routing, the default API route, and JSON-only responses.
        /// </summary>
        /// <param name="config">The HTTP configuration to configure.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Force Web API to trust and read the "Bearer" token principal verified by OWIN
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Filters.Add(new Filters.ValidateModelAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}

