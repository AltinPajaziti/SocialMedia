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
        private readonly IMySessionService _MySessionService;


        public UsersController(IRepositoryWrapper repository, IMySessionService MySessionService)
        {
            _MySessionService = MySessionService;
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
                var userLoggedIn = _MySessionService.GetUserId();  // Fix variable name (userlooggedin to userLoggedIn)

                var allUsers = await _repository.Users.GetAll().ToListAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    return NotFound("No users available for friend suggestions.");
                }

                Random random = new Random();
                var randomSuggestions = allUsers
                    .OrderBy(x => random.Next())  // Randomize the order
                    .Take(3)  // Take the first 3 after randomizing
                    .Select(async u => new {  // Use async here to await FollowRequests asynchronously
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Email = u.Email,
                        Followersent =  (await _repository.FollowRequests.GetByCondition(x => x.ReceiverId == u.Id && x.SenderId == userLoggedIn).FirstOrDefaultAsync()) != null // Checks if follow request exists
                    })
                    .ToList();

                // Since you're using `async`, you need to await `randomSuggestions` 
                var results = await Task.WhenAll(randomSuggestions);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }














    }
}

