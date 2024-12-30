using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedoa.core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories
{
    public class TokenRepo : IToken
    {
        private IConfiguration _configuration;



        public string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                throw new SecurityTokenException("Invalid token.");

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;

        }




        public TokenRepo(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string CreateToken(User user)
        {
            string token = _configuration["TokenKey"];

            if (token.Length < 64)
            {
                return "Not long egnof";
            }
            var tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));



            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Roli.RoleName )
            };

            var creds = new SigningCredentials(tokenkey, SecurityAlgorithms.HmacSha512Signature);

            var distributor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds

            };

            var jwt = new JwtSecurityTokenHandler();
            var finaltoken = jwt.CreateToken(distributor);

            return jwt.WriteToken(finaltoken);



        }
    }


}
