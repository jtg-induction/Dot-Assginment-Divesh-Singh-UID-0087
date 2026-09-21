using Moq;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Services
{
    [TestClass]
    public class ObtainJwtServiceTest
    {
        private ObtainJwtService _obtainJwtService;
        [TestInitialize]
        public void setup()
        {
            Environment.SetEnvironmentVariable("SECRET_KEY", "YourSuperSecretKeyThatIsAtLeast32BytesLong!");
            Environment.SetEnvironmentVariable("ISSUER", "TestRestaurantIssuer");

            _obtainJwtService = new ObtainJwtService();
        }
        [TestMethod]
        public void Check_CraftJwt()
        {
            User user = new User
            {
                UserId = 1,
                Name = "gggg",
                Role = UserRole.Customer
            };
           var token= _obtainJwtService.CraftJwt(user);
            Assert.IsNotEmpty(token);
        }

    }
}
