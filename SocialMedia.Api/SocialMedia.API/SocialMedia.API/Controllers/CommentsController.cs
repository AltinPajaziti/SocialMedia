using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.DTOs;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;
using System.Xml.Linq;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {



        private readonly IRepositoryWrapper _repository;
        private readonly IMySessionService _MySessionService;
        private readonly SocialMediaDbContext _dbContext;



        public CommentsController(IRepositoryWrapper repository, IMySessionService MySessionService, SocialMediaDbContext dbContext)
        {
            _repository = repository;
            _MySessionService = MySessionService;
            _dbContext = dbContext;
        }

        [HttpGet("Get-Post-Comments")]
        public async Task<IActionResult> GetAllCommentsForPost(int postid)
        {
            try
            {
                var postcomments = await _repository.posts.GetAll().Include(pc => pc.Comments).Where(p => p.Id == postid && p.Deleted == false).ToListAsync();

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


        [HttpDelete("DeletePostComments")]

        public async Task<IActionResult> DeleteComment(int commentid)
        {
            try
            {
                var postcomments = await _repository.comments.GetByCondition(c => c.Id == commentid && c.Deleted == false).FirstOrDefaultAsync();
                if (postcomments != null)
                {
                    postcomments.Deleted = true;
                    await _repository.comments.Update(postcomments);
                    await _repository.SaveAsync();
                }
                return Ok("There is not any comments");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Create-A-Comment")]
        public async Task<IActionResult> AddComment(int postId, [FromBody] CommentDto userComment)
        {
            try
            {
                if (userComment == null || string.IsNullOrWhiteSpace(userComment.Content))
                {
                    return BadRequest("Invalid comment data.");
                }

                var post = await _repository.posts
                    .GetAll()
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == postId && !p.Deleted);

                if (post == null)
                {
                    return NotFound("Post not found or has been deleted.");
                }

                var comment = new Comments
                {
                    Content = userComment.Content,
                    PostId = postId,
                    IsActive = true
                };

                await _repository.comments.Create(comment);
                await _repository.SaveAsync();

                var createdComment = new
                {
                    comment.Id,
                    comment.Content,
                    comment.PostId,
                    comment.IsActive,
                };

                return Ok(new { Message = "Comment added successfully.", Comment = createdComment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");




            }
        }
    }
}