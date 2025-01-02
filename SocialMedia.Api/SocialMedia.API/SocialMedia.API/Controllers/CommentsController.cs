using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;

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







        


        }
}
