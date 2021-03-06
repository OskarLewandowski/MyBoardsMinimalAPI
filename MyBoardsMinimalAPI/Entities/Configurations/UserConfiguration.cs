using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //relations one-to-one  User--[1]----[1]--Address
            builder.HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            //index
            //builder.HasIndex(u => u.Email);
            //composite index
            //builder.HasIndex(u => new { u.Email, u.FullName })
            //    .HasDatabaseName("FirstIndexUser") name of index
            //    .IsUnique(); unique value

            builder.HasIndex(u => new { u.Email, u.FullName });
        }
    }
}
