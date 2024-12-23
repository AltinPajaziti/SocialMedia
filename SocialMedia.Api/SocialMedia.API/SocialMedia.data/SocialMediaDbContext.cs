using Microsoft.EntityFrameworkCore;
using System;

namespace SocialMedia.Data
{
    public class SocialMediaDbContext : DbContext
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options) { }

     
    }
}
