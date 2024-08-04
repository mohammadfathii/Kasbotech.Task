using BlogAPI.Data;
using BlogAPI.Shares.Models.Auth;
using BlogAPI.Shares.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Services.IServices;
using System.Security.Cryptography;

namespace BlogAPI.Services
{
    public class UserService : IUserService
    {

        private IConfiguration _configuration;
        private AppDBContext _context;
        private PasswordHasher<object> _passwordHasher;
        public UserService(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _context = context;
            _passwordHasher = new PasswordHasher<object>();

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
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

            if (u == null || u.RefreshToken != user.RefreshToken || u.RefreshTokenExpire <= DateTime.Now)
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
