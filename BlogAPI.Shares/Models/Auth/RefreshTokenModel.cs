using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Shares.Models.Auth
{
    public class RefreshTokenModel
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}
