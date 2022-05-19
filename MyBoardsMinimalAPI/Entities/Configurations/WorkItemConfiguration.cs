using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.Property(x => x.Area).HasColumnType("varchar(200)");
            builder.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
            builder.Property(x => x.Priority).HasDefaultValue(3);

            //relations one-to-many  WorkItem--[1]----[*]--Comment
            builder.HasMany(wi => wi.Comments)
            .WithOne(c => c.WorkItem)
            .HasForeignKey(c => c.WorkItemId);

            //relations many-to-one  WorkItem--[*]----[1]--User
            builder.HasOne(wi => wi.Author)
            .WithMany(u => u.WorkItems)
            .HasForeignKey(wi => wi.AuthorId);

            //relations many-to-many  WorkItem--[*]----[*]--Tag
            builder.HasMany(w => w.Tags)
            .WithMany(t => t.WorkItems)
            .UsingEntity<WorkItemTag>
            (

                //for Tag
                w => w.HasOne(wit => wit.Tag)
                .WithMany()
                .HasForeignKey(wit => wit.TagId),

                //for WorkItem
                w => w.HasOne(wit => wit.WorkItem)
                .WithMany()
                .HasForeignKey(wit => wit.WorkItemId),

                //for WorkItemTag
                wit =>
                {
                    wit.HasKey(x => new { x.TagId, x.WorkItemId });
                    wit.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                }

             );

            //relations many-to-one  WorkItem--[*]----[1]--WorkItemState
            builder.HasOne(w => w.State)
            .WithMany()
            .HasForeignKey(w => w.StateId);
        }
    }
}
