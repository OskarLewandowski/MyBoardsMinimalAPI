using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities.Configurations
{
    public class WorkItemStateConfiguration : IEntityTypeConfiguration<WorkItemState>
    {
        public void Configure(EntityTypeBuilder<WorkItemState> builder)
        {
            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(60);

            //seed data in WorkiItemState
            builder.HasData(
                new WorkItemState { Id = 1, Value = "To do" },
                new WorkItemState { Id = 2, Value = "Doing" },
                new WorkItemState { Id = 3, Value = "Done" }
                );
        }
    }
}
