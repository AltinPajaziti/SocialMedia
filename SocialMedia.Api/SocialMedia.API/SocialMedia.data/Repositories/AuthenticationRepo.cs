
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SocialMedia.data.DTOs;

namespace SocialMedia.data.Repositories
{
    public class AuthenticationRepo : Authentication
    {
        private SocialMediaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IToken _Token { get; set; }


        public AuthenticationRepo(SocialMediaDbContext context, IToken Token, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _Token = Token;
            _httpContextAccessor = httpContextAccessor;

        }


        public Task<RefreshToken> GenerateRefreshToken()
        {
            var refreshtoken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TokenExpires = DateTime.UtcNow.AddDays(7),
                TokenCreated = DateTime.Now
            };

            return Task.FromResult(refreshtoken);
        }



        private bool ValidateRefreshToken(string token)
        {

            return !string.IsNullOrEmpty(token);
        }

        private void RefreshAccessToken(string refreshToken)
        {
            Console.WriteLine($"Access token refreshed using the refresh token: {refreshToken}");
        }


        public async void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = refreshToken.TokenExpires,
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshtoken", refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.TokenExpires;
            user.TokenCreated = refreshToken.TokenCreated;

            await _context.SaveChangesAsync();


        }


        public async Task<LoginUserDto> LoginAsync(LoginDto loginDto)
        {
            var User = _context.Users.Include(ur => ur.Roli).FirstOrDefault(u => u.Name == loginDto.Username);
            if (User == null)
            {
                throw new Exception("User not in Database");
            }

            var hmac = new HMACSHA512(User.PasswordSalt);


            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));


            for (var i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != User.PasswordHash[i])
                {
                    throw new Exception("Incorrect password");
                }
            }



            var refreshToken = await GenerateRefreshToken();


            var loginUserDto = new LoginUserDto
            {
                Username = loginDto.Username,
                Token = _Token.CreateToken(User),
                Role = (User.Roleid == 2) ? "Admin" : "User",





                Status = "ok"
            };

            SetrefreshToken(refreshToken, loginUserDto);






            return loginUserDto;

        }


        public async void SetrefreshToken(RefreshToken refreshToken, LoginUserDto user)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = refreshToken.TokenExpires,
                SameSite = SameSiteMode.Strict
            };
            var Theuser = _context.Users.Where(u => u.Name == user.Username).FirstOrDefault();

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshtoken", refreshToken.Token, cookieOptions);

            Theuser.RefreshToken = refreshToken.Token;
            Theuser.TokenExpires = refreshToken.TokenExpires;
            Theuser.TokenCreated = refreshToken.TokenCreated;

            _context.SaveChangesAsync();

            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.TokenExpires;
            user.TokenCreated = refreshToken.TokenCreated;



        }

        public async Task<User> RegisterAsync(RegisterDto register)
        {
            var User = _context.Users.FirstOrDefault(u => u.Name == register.Name);
            if (User != null)
            {
                throw new Exception("User is alredy Registerd");
            }

            var hmac = new HMACSHA512();

            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password));

            var Register = new User
            {
                Name = register.Name,
                Surname = register.Surname,
                Adress = register.Adress,
                Email = register.Email,
                //PasswordSalt = hmac.Key,
                //PasswordHash = passwordHash,
                Roleid = 2

            };

            _context.Users.Add(Register);

            await _context.SaveChangesAsync();

            return Register;
        }


    }

}
