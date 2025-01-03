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



        [HttpDelete("DeletePost")]

        public async Task<IActionResult> DeleteComment(int Postid)
        {
            try
            {
                var postcomments = await _repository.posts.GetByCondition(c => c.Id == Postid && c.Deleted == false).FirstOrDefaultAsync();
                if (postcomments != null)
                {
                    postcomments.Deleted = true;
                    await _repository.posts.Update(postcomments);
                    await _repository.SaveAsync();
                }
                return Ok("There is not any comments");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Create-A-Post")]
        public async Task<IActionResult> AddPost([FromBody] PostDto userPost)
        {
            try
            {
                var userId = _MySessionService.GetUserId();

                if (userPost == null || string.IsNullOrWhiteSpace(userPost.Content) || string.IsNullOrWhiteSpace(userPost.Title))
                {
                    return BadRequest("Invalid post data.");
                }

                var user = await _repository.Users
                    .GetByCondition(u => u.Id == userId)
                    .Include(u => u.Posts)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                var newPost = new Post
                {
                    Content = userPost.Content,
                    Title = userPost.Title,
                    IsActive = true,
                    InsertedOn = DateTime.UtcNow,
                    UserId = userId
                };

                user.Posts.Add(newPost);

                await _repository.Users.Update(user);
                await _repository.posts.Update(newPost);
                await _repository.SaveAsync();

                return Ok(new { Message = "Post created successfully.", Post = newPost });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpPost("Update-Post")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDto updatedPost)
        {
            try
            {
                var userId = _MySessionService.GetUserId();

                if (updatedPost == null || string.IsNullOrWhiteSpace(updatedPost.Content) || string.IsNullOrWhiteSpace(updatedPost.Title))
                {
                    return BadRequest("Invalid post data.");
                }

                var post = await _repository.posts
                    .GetByCondition(p => p.Id == postId && p.UserId == userId)
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return NotFound("Post not found or you do not have permission to update this post.");
                }

                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;
                post.LastModified = DateTime.UtcNow;

                _repository.posts.Update(post);
                await _repository.SaveAsync();

                return Ok(new { Message = "Post updated successfully.", Post = post });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
