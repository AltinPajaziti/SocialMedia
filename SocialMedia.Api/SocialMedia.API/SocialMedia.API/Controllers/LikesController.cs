using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.DTOs;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {

        private readonly IRepositoryWrapper _repository;
        private readonly IMySessionService _MySessionService;
        private readonly SocialMediaDbContext _dbContext;



        public LikesController(IRepositoryWrapper repository, IMySessionService MySessionService, SocialMediaDbContext dbContext)
        {
            _repository = repository;
            _MySessionService = MySessionService;
            _dbContext = dbContext;
        }

        [HttpGet("Get-Post-Likes")]
        public async Task<IActionResult> GetallLikes(int postid)
        {
            try
            {
                var postcomments = await _repository.posts.GetAll().Include(pc => pc.Likes).Where(p => p.Id == postid && p.Deleted == false).ToListAsync();

                if (postcomments != null)
                {
                    return Ok(postcomments);
                }
                return Ok("there are not any comments");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeletePostLikes")]

        public async Task<IActionResult> DeleteLikes(int likeid)
        {
            try
            {
                var postlikes = await _repository.likes.GetByCondition(c => c.Id == likeid && c.Deleted == false).FirstOrDefaultAsync();
                if (postlikes != null)
                {
                    postlikes.Deleted = true;
                    await _repository.likes.Update(postlikes);
                    await _repository.SaveAsync();
                }
                return Ok("There is not any comments");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add-Like")]
        public async Task<IActionResult> AddLike(int postId)
        {
            try
            {
                var post = await _repository.posts
                    .GetByCondition(p => p.Id == postId && p.IsActive)
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return NotFound("Post not found or inactive.");
                }

                var like = new Likes
                {
                    Postid = postId,
                    posts = post,
                    IsActive = true
                };

                await _repository.likes.Create(like);
                await _repository.SaveAsync();

                var createdLike = new
                {
                    like.Id,
                    like.Postid,
                    like.IsActive
                };

                return Ok(new { Message = "Like added successfully.", Like = createdLike });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    
}
}
