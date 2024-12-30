using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.DTOs;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;
using System.Security.Claims;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public Authentication _Authentication;
        private SocialMediaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToken _token;



        public AuthenticationController(Authentication Authentication, SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor, IToken token)
        {
            _Authentication = Authentication;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _token = token;
        }




        [HttpPost("Register")]

        public async Task<ActionResult<User>> Register([FromBody] RegisterDto register)
        {
            if (register == null)
            {
                return BadRequest("register is empty");
            }

            try
            {
                var user = await _Authentication.RegisterAsync(register);

                if (user == null)
                {
                    return BadRequest("The user did not register");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("The username is null");
            }

            try
            {
                var userDto = await _Authentication.LoginAsync(loginDto);

                if (userDto == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh-token"), Authorize]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var userid = Int32.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.Include(ur => ur.Roli).Where(u => u.Id == userid).FirstOrDefaultAsync();
            var refreshToken = Request.Cookies["refreshtoken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }


            string token = _token.CreateToken(user);
            var newRefreshToken = await _Authentication.GenerateRefreshToken();
            _Authentication.SetRefreshToken(newRefreshToken, user);

            return Ok(token);
        }





    }
}

