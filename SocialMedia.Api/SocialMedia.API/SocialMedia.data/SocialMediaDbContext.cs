using Microsoft.EntityFrameworkCore;
using SocialMedoa.core;
using System;

namespace SocialMedia.Data
{
    public class SocialMediaDbContext : DbContext
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options) { }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }

        public virtual DbSet<Comments> Comments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasOne(r => r.Roli)
            .WithMany(u => u.User)
            .HasForeignKey(f => f.Roleid)
            .OnDelete(DeleteBehavior.Cascade);


            

        }




    }
}
