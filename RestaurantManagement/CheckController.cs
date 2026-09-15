using RestaurantManagement.Data;
using System.Web.Http;


namespace RestaurantManagement
{
    /// <summary>
    /// Provides an endpoint for checking database availability.
    /// </summary>
    [RoutePrefix("api/check")]
    public class CheckController : ApiController
    {
        /// <summary>
        /// Checks whether the configured database exists and writes the result to the debug output.
        /// </summary>
        [HttpGet]
        [Route("")]
        public void Check()
        {
            var check = new ApplicationDbContext().Database.Exists();
            System.Diagnostics.Debug.WriteLine("Database exists: " + check);
        }
    }
}

