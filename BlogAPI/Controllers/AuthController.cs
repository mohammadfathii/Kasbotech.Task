using BlogAPI.Services.IServices;
using BlogAPI.Shares.Models.Auth;
using BlogAPI.Shares.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserService _JWTAuthenticationRepository;
        public AuthController(IUserService JWTAuthentication)
        {
            _JWTAuthenticationRepository = JWTAuthentication;
        }
        [HttpPost("/Auth/Login")]
        public NewTokenModel Login(string UserName, string Password)
        {
            var user = new Shares.Models.Auth.LoginModel()
            {
                UserName = UserName,
                Password = Password
            };
            var token = _JWTAuthenticationRepository.login(user);

            if (token == null)
            {
                return new NewTokenModel()
                {
                    AccessToken = "User Not Founded",
                    RefreshToken = "User Not Founded"
                };
            }

            return token;
        }

        [HttpPost("/Auth/Register")]
        public bool Register(string UserNmae,string Password)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var register = _JWTAuthenticationRepository.register(new RegisterModel()
            {
                UserName = UserNmae,
                Password = Password
            });

            if (register == false)
            {
                return false;
            }

            return true;
        }

        [HttpPost("/Auth/Refresh")]
        public NewTokenModel RefreshToken(RefreshTokenModel user)
        {
            var login = _JWTAuthenticationRepository.refreshToken(user);

            return login;
        }

    }
}
