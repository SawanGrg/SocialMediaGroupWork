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

        // OnModelCreating is used to configure the model in the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
        }


        // DbSet defines the table name and the model to be used
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId); // Set the primary key

            base.OnModelCreating(modelBuilder);
        }
    }
}
