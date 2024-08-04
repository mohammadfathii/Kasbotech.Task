using BlogAPI.Services.Repositories;
using BlogAPI.Shares.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IJWTAuthenticationService _JWTAuthenticationRepository;
        public AuthController(IJWTAuthenticationService JWTAuthentication)
        {
            _JWTAuthenticationRepository = JWTAuthentication;
        }
        [HttpGet]
        public string Authentication(string UserName, string Password)
        {
            var user = new Shares.Models.Auth.LoginModel()
            {
                UserName = UserName,
                Password = Password
            };
            var token = _JWTAuthenticationRepository.login(user);

            if (token == null)
            {
                return "Not Founded User";
            }

            return token.AccessToken;
        }
    }
}
