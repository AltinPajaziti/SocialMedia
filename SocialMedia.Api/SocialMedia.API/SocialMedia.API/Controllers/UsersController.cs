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
                var userLoggedIn = _MySessionService.GetUserId();  // Get the logged-in user ID

                var allUsers = await _repository.Users.GetAll().ToListAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    return NotFound("No users available for friend suggestions.");
                }

                Random random = new Random();

                // Fetch all the necessary follow request data outside of LINQ
                var followRequests = await _repository.FollowRequests.GetAll()
                    .Where(x => allUsers.Select(u => u.Id).Contains(x.ReceiverId) && x.SenderId == userLoggedIn)
                    .ToListAsync();

                // Randomize and take top 3 users
                var randomSuggestions = allUsers
                    .OrderBy(x => random.Next())  // Randomize the order
                    .Take(3)  // Take the first 3 after randomizing
                    .Select(u => new
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Email = u.Email,
                        // Check if a follow request was sent to this user
                        Followersent = followRequests.Any(fr => fr.ReceiverId == u.Id)
                    })
                    .ToList();

                return Ok(randomSuggestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }














    }
}

