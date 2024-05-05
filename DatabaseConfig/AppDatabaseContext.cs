using Microsoft.EntityFrameworkCore;
using GroupCoursework.Models;
using Microsoft.Extensions.Configuration;

namespace GroupCoursework.DatabaseConfig
{
    public class AppDatabaseContext : DbContext
    {
        public IConfiguration _configuration { get; set; }

        public AppDatabaseContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogVote> BlogVotes { get; set; }
        public DbSet<BlogComments> BlogComments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<BlogHistory> BlogHistory { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<CommentHistory> CommentHistory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId); // Set the primary key

            base.OnModelCreating(modelBuilder);
        }
    }
}
