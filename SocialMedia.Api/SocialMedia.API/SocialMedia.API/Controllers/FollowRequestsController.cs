using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.data.Services;
using SocialMedia.Data;
using SocialMedoa.core;
using System.Security.Claims;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowRequestsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMySessionService _MySessionService;
        private readonly SocialMediaDbContext _dbContext;
        


        public FollowRequestsController(IRepositoryWrapper repository , IMySessionService MySessionService , SocialMediaDbContext dbContext)
        {
            _repository = repository;
            _MySessionService = MySessionService;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllFollowRequests")]

        public async Task<IActionResult> GetAllFollowRequests()
        {
            try
            {
                var userid = _MySessionService.GetUserId();

                var followers = await _repository.FollowRequests.GetAll().Where(u => u.SenderId == userid).ToListAsync();
                return Ok(followers);
            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        [HttpGet("AcceptFollow")]

        public async Task<IActionResult> AcceptFollow(int Followid)
        {
            try
            {
                var userid = _MySessionService.GetUserId();

                var TheFollow =await _repository.FollowRequests.GetByCondition(x => x.Id == Followid).FirstOrDefaultAsync();

                if(userid == TheFollow.ReceiverId)
                {
                    TheFollow.Status = SocialMedoa.core.FollowRequestStatus.Accepted;

                    _repository.FollowRequests.Update(TheFollow);

                    await _repository.SaveAsync();

                    return Ok("Follow Accepted succesfully");
                }

                return BadRequest("This follow it is not for you");

                


            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("DeclineFollow")]

        public async Task<IActionResult> DeclineFollow(int Followid)
        {
            try
            {
                var TheFollow = await _repository.FollowRequests.GetByCondition(x => x.Id == Followid).FirstOrDefaultAsync();
                var Username = _MySessionService.GetUserId();
                TheFollow.Status = SocialMedoa.core.FollowRequestStatus.Declined;
                TheFollow.LastModified = DateTime.Now;
                TheFollow.ModifiedBy = Username;

                TheFollow.Deleted = true;

                _repository.FollowRequests.Update(TheFollow);

                await _repository.SaveAsync();

                return Ok("Follow decline succesfully");


            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpPost("RequestForFollow")]


        public async Task<IActionResult> RequestForFollow(int Followid)
        {

            try
            {
                var userid = 2;

                var sender = await _repository.Users.GetAll()
                    .Include(x => x.SentFollowRequests)
                    .FirstOrDefaultAsync(u => u.Id == userid);

                if (sender == null)
                    return NotFound("Sender not found.");

                var receiver = await _repository.Users.GetAll().FirstOrDefaultAsync(u => u.Id == Followid);
                if (receiver == null)
                    return NotFound("Receiver not found.");

                var existingRequest = await _repository.FollowRequests.GetAll()
                    .FirstOrDefaultAsync(fr => fr.SenderId == sender.Id && fr.ReceiverId == receiver.Id);

                if (existingRequest != null)
                {
                    return BadRequest("A follow request already exists between these users.");
                }

                var followRequest = new FollowRequests
                {
                    SenderId = sender.Id,
                    ReceiverId = receiver.Id,
                    Status = FollowRequestStatus.Pending, 
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.FollowRequests.Create(followRequest);
                await _repository.SaveAsync();

                return Ok("Follow request sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




















    }
}
