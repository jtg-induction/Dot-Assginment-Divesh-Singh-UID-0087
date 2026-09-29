using System.Web.Http;
using Unity.AspNet.WebApi;
namespace RestaurantManagement
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

          


        }
    }
}
