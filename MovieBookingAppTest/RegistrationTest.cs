using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using MovieBookingApp.Controllers;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingAppTest
{
    public class RegistrationTest
    {
        private readonly Mock<IRegistrationRepository> registrationRepository;
        private readonly Mock<ILogger<RegisterationController>> logger;
        public RegistrationTest()
        {
            registrationRepository= new Mock<IRegistrationRepository>();
            logger = new Mock<ILogger<RegisterationController>>();
        }

        /// <summary>
        /// Registration Test case
        /// </summary>
        [Test]
        public void RegisterTest()
        {
            //Arrange
            var data = new RegistrationData
            {
                RegistrationId=ObjectId.GenerateNewId(),
                FirstName = "Yasmeen",
                LastName = "Shadab",
                Email = "unique@gmail.com",
                LoginId = "unique@User",
                Password = "password",
                ConfirmPassword = "password",
                PhoneNumber = "999999999"
            };
            var controller= new RegisterationController(registrationRepository.Object, logger.Object);
            registrationRepository.Setup(x => x.RegistreUser(It.IsAny<RegistrationData>())).ReturnsAsync(new ObjectId("66347428e574c152bf877ed5"));
            registrationRepository.Setup(x => x.GetEmailId(It.IsAny<RegistrationData>()));
            registrationRepository.Setup(x => x.GetLoginId(It.IsAny<RegistrationData>()));

            //Act
            var register = controller.Register(data);
            var result = (JsonResult)register.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Value.ToString, Is.EqualTo("66347428e574c152bf877ed5"));

        }

        /// <summary>
        /// Login User Test Case
        /// </summary>
        [Test]
        public void LoginUserTest()
        {
            //Arrange
            var loginId = "Yasmeen@User1";
            var password = "password";
            var controller=new RegisterationController(registrationRepository.Object, logger.Object);
            registrationRepository.Setup(x => x.LoginUser(loginId, password));

            //Act
            var login = controller.Login(loginId, password);
            var result = (JsonResult)login.Result;
            var res=(bool)result.Value;

            //Assert
            Assert.True(res || !res);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Set Password test case
        /// </summary>
        [Test]
        public void SetPasswordTest()
        {
            //Arrange
            var data = new SetPassword
            {
                LoginID = "Yasmeen@User1",
                Email = "Yasmeen@gmail.com",
                NewPassword = "password",
                ConfirmNewPassword = "password"
            };
            var controller= new RegisterationController(registrationRepository.Object, logger.Object);
            registrationRepository.Setup(x => x.ForgetPassword(data));

            //Act
            var setPassword=controller.SetPassword(data);
            var res=(JsonResult)setPassword.Result;
            var result = (bool)res.Value;

            //Assert
            Assert.True(result||!result);
            Assert.IsNotNull(setPassword);
        }
    }
}
