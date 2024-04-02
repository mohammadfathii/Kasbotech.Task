using BlogAPI.Models;

namespace BlogAPI.Services.Repositories
{
    public interface IJWTAuthenticationRepository
    {
        TokenModel Authenticate(UserModel user);
    }
}
