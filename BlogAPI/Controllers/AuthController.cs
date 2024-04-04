using BlogAPI.Models;
using BlogAPI.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IJWTAuthenticationRepository _JWTAuthenticationRepository;
        public AuthController(IJWTAuthenticationRepository JWTAuthentication)
        {
            _JWTAuthenticationRepository = JWTAuthentication;
        }
        [HttpGet]
        public string Authentication(string UserName,string Password)
        {
            var user = new UserModel()
            {
                UserName = UserName,
                Password = Password
            };
            var token = _JWTAuthenticationRepository.Authenticate(user);

            if (token == null)
            {
                return "Not Founded User";
            }

            return token.Token;
        }
    }
}
