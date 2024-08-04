using BlogAPI.Shares.Models.Auth;
using BlogAPI.Shares.Models;

namespace BlogAPI.Services.IServices
{
    public interface IUserService
    {
        string GenerateToken(int id, string username);
        string GenerateRefreshToken();
        bool register(RegisterModel user);
        NewTokenModel login(LoginModel user);
        NewTokenModel refreshToken(RefreshTokenModel user);
    }
}
