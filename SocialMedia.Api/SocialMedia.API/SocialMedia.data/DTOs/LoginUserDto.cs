using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.DTOs
{
    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public string Status { get; set; } = string.Empty;
        public string Role { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
