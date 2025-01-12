using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.Repositories.Interfaces;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public UsersController(IRepositoryWrapper repository)
        {
            _repository = repository;

        }


        [HttpGet("Get-all-users") ]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _repository.Users.GetAll().FirstOrDefaultAsync();
                return Ok(users);
            }

            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching users.", error = ex.Message });
            }
        }



        [HttpGet("GetSugestionFrends")]
        public async Task<IActionResult> GetAllSuggestions()
        {
            try
            {
                var allUsers = await _repository.Users.GetAll().ToListAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    return NotFound("No users available for friend suggestions.");
                }

                Random random = new Random();
                var randomSuggestions = allUsers.OrderBy(x => random.Next()).Take(3).ToList(); 

                return Ok(randomSuggestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }














    }
}

