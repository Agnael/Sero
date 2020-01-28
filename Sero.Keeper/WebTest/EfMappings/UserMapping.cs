using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.EfMappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("user_account");

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.HasIndex(x => x.Name)
                .IsUnique();
            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(20)
                .IsRequired();

            builder.Ignore(x => x.IsDeleted);
            //builder.Property(x => x.IsDeleted)
            //    .HasColumnName("is_deleted");

            builder.HasMany(x => x.NickNames)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
