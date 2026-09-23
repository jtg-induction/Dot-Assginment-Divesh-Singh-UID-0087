using Moq;
using RestaurantManagement.Controllers;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace RestaurantManagement.tests.Controller
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController _accountController;
        private  Mock<IUserService> _userservicemock;
        private Mock<IClaimHelper> _claimhelpermock;
        [TestInitialize]
        public void setup()
        {
            _userservicemock = new Mock<IUserService>();
            _claimhelpermock = new Mock<IClaimHelper>();
            _accountController = new AccountController(_userservicemock.Object,_claimhelpermock.Object);
        }
        [TestMethod]
        public async Task updateaccount()
        {
            UpdateAccountDto update = new UpdateAccountDto
            {
                Email = "abc@abc.com"
            };

            _claimhelpermock.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);
            _userservicemock.Setup(e => e.GetUserIfActive(1)).ReturnsAsync(It.IsAny<User>());
            _userservicemock.Setup(e => e.UpdateAccount(It.IsAny<User>(),update));
          
              var response= await  _accountController.UpdateAccount(update);
            //Assert.Fail(response.GetType().FullName);
           var okk= response as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(okk);

        }

    }
}
