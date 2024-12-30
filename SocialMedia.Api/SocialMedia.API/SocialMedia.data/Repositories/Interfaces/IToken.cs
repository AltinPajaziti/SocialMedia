using SocialMedoa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories.Interfaces
{
    public interface IToken
    {
        string CreateToken(User user);

        string GetUserIdFromToken(string token);
    }
}
