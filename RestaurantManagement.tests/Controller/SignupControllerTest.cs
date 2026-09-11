using Microsoft.Extensions.DependencyModel;
using Moq;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.repository;
using RestaurantManagement.services;
using RestaurantManagement.Services;
using System.Web.Http.Results;
using System.Web.UI.WebControls.WebParts;

namespace RestaurantManagement.tests.Controller
{
    [TestClass]
    public class SignupControllerTest
    {

        private Mock<IUserService> _mockser;
        private SignUpController _signup;
        [TestInitialize]
        public void setup()
        {
            _mockser = new Mock<IUserService>();
            _signup = new SignUpController(_mockser.Object);
        }
             
        [TestMethod]
        public void all_correct_detail()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "divesh@gmail.com",
                Password = "123234@aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
            var response = _signup.signup(incominguser);
            //ASSERT
            var createdResult = response as CreatedNegotiatedContentResult<AddUserRequest>;
            Assert.IsNotNull(createdResult, "succes");
            Assert.AreEqual("DIVESH", createdResult.Content.Name);
        }

        [TestMethod]
        public void Invalidemail()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "divesgmail.com",
                Password = "123234@aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
            var response = _signup.signup(incominguser);


            //ASSERT
            var createdResult = response as BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);
            //System.Diagnostics.Debug.WriteLine("b jkbhbh",response);

            //Assert.IsNotNull(createdResult);
            //Assert.Fail("The actual response type is: " + response.GetType().FullName);
            //ARRANGE
            incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = null,
                Password = "123234@aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
           response = _signup.signup(incominguser);


            //ASSERT
            createdResult = response as BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);


        }

        [TestMethod]
        public void InvalidPhonenumber()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "divesh@gmail.com",
                Password = "123234@aA",
                PhoneNumber = null,
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
            var response = _signup.signup(incominguser);


            //ASSERT
            var createdResult = response as System.Web.Http.Results.BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);


        }
        [TestMethod]
        public void NothingToBeNUll()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                
                Email = "dives@gmail.com",
                Password = "123234@aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");


            //ACT
            var response = _signup.signup(incominguser);


            //ASSERT
            var createdResult = response as BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);


        }
        [TestMethod]
        public void Invalidpassword()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "dives@gmail.com",
                Password = "123234aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
            var response = _signup.signup(incominguser);


            //ASSERT
       
            var createdResult = response as System.Web.Http.Results.BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);


        }
         //as you put invalid birthdate then it occur as "0001-01-01 00:00:00"
        [TestMethod]
        public void InvalidBirthDate()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "dives@gmail.com",
                Password = "123234@aA",
                PhoneNumber = "12323342",
                BirthDate = DateTime.Parse("0001-01-01 00:00:00")
            };

            _mockser.Setup(r => r.Adduser(incominguser)).Returns("ok");

            //ACT
            var response = _signup.signup(incominguser);


            //ASSERT
  
            var createdResult = response as System.Web.Http.Results.BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);

            Assert.AreEqual("Nothing to be null and check email and password", createdResult.Message);
        }
       
       

    }
}
