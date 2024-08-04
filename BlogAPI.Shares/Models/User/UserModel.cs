using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Shares.Models.User
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [MaybeNull]
        public string RefreshToken { get; set; } = string.Empty;
        [MaybeNull]
        public DateTime RefreshTokenExpire { get; set; }
    }
}
