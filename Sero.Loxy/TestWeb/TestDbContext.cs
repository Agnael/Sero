using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestWeb.Models;

namespace TestWeb
{
    public class TestDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public TestDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlite("Data Source=sqlite.db");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(50);
        }
    }
}
