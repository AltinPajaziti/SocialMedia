using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMySessionService _MySessionService;
        private readonly SocialMediaDbContext _dbContext;



        public PostsController(IRepositoryWrapper repository, IMySessionService MySessionService, SocialMediaDbContext dbContext)
        {
            _repository = repository;
            _MySessionService = MySessionService;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllFriendsPosts")]
        public async Task<IActionResult> GetAllFriendsPosts()
        {
            var userId = _MySessionService.GetUserId();

            var loggedInUser = await _repository.Users
                .GetByCondition(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (loggedInUser == null)
            {
                return BadRequest("The user does not exist.");
            }

            var friendIds = await _repository.FollowRequests.GetAll()
                .Where(fr =>
                    (fr.SenderId == userId || fr.ReceiverId == userId) &&
                    fr.Status == FollowRequestStatus.Accepted)
                .Select(fr => fr.SenderId == userId ? fr.ReceiverId : fr.SenderId)
                .Distinct()

                .ToListAsync();

            var friendsPosts = await _repository.posts.GetAll()
                .OrderByDescending(x => x.InsertedOn)
                .Where(post => friendIds.Contains(post.UserId))
                .ToListAsync();

            return Ok(friendsPosts);
        }









    }
}
