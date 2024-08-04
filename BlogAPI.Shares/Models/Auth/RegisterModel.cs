using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Shares.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
