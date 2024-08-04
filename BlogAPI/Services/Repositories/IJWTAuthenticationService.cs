using BlogAPI.Shares.Models.Auth;
using BlogAPI.Shares.Models;

namespace BlogAPI.Services.Repositories
{
    public interface IJWTAuthenticationService
    {
        string GenerateToken(int id, string username);
        string GenerateRefreshToken();
        bool register(RegisterModel user);
        NewTokenModel login(LoginModel user);
        NewTokenModel refreshToken(RefreshTokenModel user);
    }
}
