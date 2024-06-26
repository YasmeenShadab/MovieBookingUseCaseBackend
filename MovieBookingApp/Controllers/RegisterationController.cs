using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    [Route("api/v1.0/moviebooking")]
    [ApiController]
    public class RegisterationController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ILogger<RegisterationController> _logger;
        public RegisterationController(IRegistrationRepository registrationRepository, ILogger<RegisterationController> logger)
        {
            _registrationRepository = registrationRepository;
            _logger = logger;
        }

        /// <summary>
        /// Registration of User
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// Registration Id
        /// </returns>
        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegistrationData data)
        {
            if (data != null)
            {
                _logger.LogInformation("Regiester : Registration Controller");
                var emailError = await _registrationRepository.GetEmailId(data);
                var loginError = await _registrationRepository.GetLoginId(data);

                if (emailError != "success" && loginError == "success")
                {
                    return new JsonResult("EmailID  must be Unique");
                }
                else if (loginError != "success" && emailError == "success")
                {
                    return new JsonResult("LoginID  must be Unique");
                }
                else if (emailError == "fail" && loginError == "fail")
                {
                    return new JsonResult("EmailID and LoginID  must be Unique");
                }
                else
                {
                    if (data.Password != data.ConfirmPassword)
                    {
                        return new JsonResult("Password and Confirm  Password must be same");
                    }
                    var id = await _registrationRepository.RegistreUser(data);

                    return new JsonResult("Sucess");
                }
            }
            _logger.LogInformation("Regiester : Registration data is null");

            return new JsonResult("Unable to Register");
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="Password"></param>
        /// <returns>
        /// true or false
        /// </returns>
        [HttpGet("login")]
        public async Task<JsonResult> Login(string loginId, string Password)
        {
            if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(Password))
            {
                _logger.LogInformation("Login : Login or Password is null");
               
                return new JsonResult("Unable to Login");
            }
            _logger.LogInformation("Login : Resitration controller");
            var result = await _registrationRepository.LoginUser(loginId, Password);
            
            return new JsonResult(result);
        }

        /// <summary>
        /// Rest Passowrd
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// true or false
        /// </returns>
        [HttpPost("forgetPassword")]
        public async Task<JsonResult> SetPassword([FromBody] SetPassword data)
        {
            if (data != null)
            {
                _logger.LogInformation("ResetPassword : Registration Controller");
                if (data.NewPassword != data.ConfirmNewPassword)
                {
                    return new JsonResult("New Password and Confirm New Password must be same");
                }
                var result = await _registrationRepository.ForgetPassword(data);

                return new JsonResult(result);
            }
            _logger.LogInformation("ResetPassword : Data is null");

            return new JsonResult("Unable to reset Password");
        }
    }
}

