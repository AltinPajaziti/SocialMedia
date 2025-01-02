using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using SocialMedoa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories
{
    public class PostRepository : GenericRepository<Post> , IPosts
    {
        public PostRepository(SocialMediaDbContext dbContext) : base(dbContext)
        {


        }
    }
}
