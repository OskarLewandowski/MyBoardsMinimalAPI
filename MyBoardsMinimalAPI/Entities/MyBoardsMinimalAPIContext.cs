using Microsoft.EntityFrameworkCore;
using MyBoardsMinimalAPI.Entities.Configurations;
using MyBoardsMinimalAPI.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class MyBoardsMinimalAPIContext : DbContext
    {
        public MyBoardsMinimalAPIContext(DbContextOptions<MyBoardsMinimalAPIContext> options) : base(options)
        {

        }

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Epic> Epic { get; set; }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }
        public DbSet<WorkItemTag> WorkItemTag { get; set; }
        public DbSet<TopAuthor> ViewTopAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //we can do it by single or
            //new AddressConfiguration().Configure(modelBuilder.Entity<Address>());
            //new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            
            //modelBuilder.Entity<WorkItemTag>()
            //    .HasKey(c => new { c.TagId, c.WorkItemId });

            //task data seed tags example 2 of 2
            //we need to do first migration and next update database for this example
            //modelBuilder.Entity<Tag>()
            //    .HasData(
            //    new Tag { Id = 1, Value = "Web" },
            //    new Tag { Id = 2, Value = "UI" },
            //    new Tag { Id = 3, Value = "Desktop" },
            //    new Tag { Id = 4, Value = "API" },
            //    new Tag { Id = 5, Value = "Service" }
            //    );

        }

        //Example of composite keys on User for LastName and Email
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasKey(x => new { x.LastName, x.Email });
        //}

        //We can add connection string here but it's not good
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=MyBoardsDb;Trusted_Connection=True;");
        //}
    }
}
