using SocialMedia.data.DTOs;
using SocialMedoa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories.Interfaces
{
    public interface Authentication
    {



        public Task<LoginUserDto> LoginAsync(LoginDto loginDto);
        public Task<User> RegisterAsync(RegisterDto register);
        public Task<RefreshToken> GenerateRefreshToken();
        void SetRefreshToken(RefreshToken refreshToken, User user);

        void SetrefreshToken(RefreshToken refreshToken, LoginUserDto user);
    }
}
