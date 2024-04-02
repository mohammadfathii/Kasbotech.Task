using BlogAPI.Models;
using BlogAPI.Services.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Services
{
    public class JWTAuthenticationRepository : IJWTAuthenticationRepository
    {

        public IDictionary<string,string> Users { get; set; } = new Dictionary<string, string>()
        {
            { "123" ,"123" },
            { "admin" ,"123" },
            { "root" ,"123" },
        };

        private IConfiguration _configuration;
        public JWTAuthenticationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public TokenModel Authenticate(UserModel user)
        {

            if (!Users.Any(u => u.Key == user.UserName && u.Value == user.Password))
            {
                return null;
            }
            // else Were : 

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier,user.UserName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return new TokenModel()
            {
                Token = tokenHandler.WriteToken(token),
            };
        }
    }
}
