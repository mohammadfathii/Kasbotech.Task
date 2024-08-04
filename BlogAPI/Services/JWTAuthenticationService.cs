using BlogAPI.Data;
using BlogAPI.Services.Repositories;
using BlogAPI.Shares.Models.Auth;
using BlogAPI.Shares.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Services
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {

        public IDictionary<string, string> Users { get; set; } = new Dictionary<string, string>()
        {
            { "123" ,"123" },
            { "admin" ,"123" },
            { "root" ,"123" },
        };

        private IConfiguration _configuration;
        private AppDBContext _context;
        private PasswordHasher<object> _passwordHasher;
        public JWTAuthenticationService(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _context = context;
            _passwordHasher = new PasswordHasher<object>();

        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public string GenerateToken(int id, string username)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserName == username);

            if (u == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier,id.ToString()),
                    new Claim(ClaimTypes.Name,username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
        public NewTokenModel login(LoginModel user)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (u == null)
            {
                return null;
            }

            var validPassword = _passwordHasher.VerifyHashedPassword(null, u.Password, user.Password);
            if (validPassword != PasswordVerificationResult.Success)
            {
                return null;
            }

            var jwtToken = GenerateToken(u.Id, u.UserName);

            var refreshToken = u.RefreshToken;
            if (u.RefreshToken == "" || u.RefreshTokenExpire <= DateTime.Now)
            {
                refreshToken = GenerateRefreshToken();
                u.RefreshToken = refreshToken;
                u.RefreshTokenExpire = DateTime.Now.AddDays(7);

                _context.Users.Update(u);
                _context.SaveChanges();
            }

            return new NewTokenModel()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        public NewTokenModel refreshToken(RefreshTokenModel user)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (u.RefreshToken != user.RefreshToken)
            {
                return null;
            }

            if (u.RefreshTokenExpire != DateTime.Now)
            {
                return null;
            }

            var jwtToken = GenerateToken(u.Id, u.UserName);
            var refreshToken = u.RefreshToken;

            return new NewTokenModel()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        public bool register(RegisterModel user)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (u != null)
            {
                return false;
            }

            var password = _passwordHasher.HashPassword(null, user.Password);

            _context.Users.Add(new Shares.Models.User.UserModel()
            {
                UserName = user.UserName,
                Password = password,
            });

            _context.SaveChanges();

            return true;
        }
    }
}
