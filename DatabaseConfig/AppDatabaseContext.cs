﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<BlogHistory> BlogsHistory { get; set; }
        //public DbSet<BlogVote> BlogVotes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId); // Set the primary key

            base.OnModelCreating(modelBuilder);
        }
    }
}
