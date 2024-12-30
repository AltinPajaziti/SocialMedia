using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
