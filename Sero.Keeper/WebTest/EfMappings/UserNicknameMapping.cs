using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.EfMappings
{
    public class UserNicknameMapping : IEntityTypeConfiguration<UserNickName>
    {
        public void Configure(EntityTypeBuilder<UserNickName> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("user_nickname");

            builder.Property(x => x.Id)
                .HasColumnName("id");
            
            builder.HasIndex(x => x.Description)
                .IsUnique();
            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.IdUser)
                .HasColumnName("id_user");

            builder.Ignore(x => x.IsDeleted);
            //builder.Property(x => x.IsDeleted)
            //    .HasColumnName("is_deleted");
        }
    }
}
