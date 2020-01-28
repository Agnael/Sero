using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Sero.Keeper.Ef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebTest.EfMappings;
using WebTest.Entities;

namespace WebTest
{
    public class AppDbContext : KeeperDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserNickName> UserNicknames { get; set; }

        public AppDbContext([NotNullAttribute] DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserMapping());
            modelBuilder.ApplyConfiguration<UserNickName>(new UserNicknameMapping());
        }

        protected override async Task OnBeforeSaveChanges(DbContext context)
        {
            
        }

        protected override async Task OnAfterSaveChanges(DbContext context, int savedId)
        {

        }
    }
}
