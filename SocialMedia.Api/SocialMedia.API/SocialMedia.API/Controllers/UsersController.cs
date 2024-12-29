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


        [HttpGet("Get-all-users")]

        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _repository.Users.GetAll().ToListAsync();
                return Ok(users);
            }

            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching users.", error = ex.Message });
            }
        }













    }
}

