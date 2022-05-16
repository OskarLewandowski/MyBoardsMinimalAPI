﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //we can write any requirement separately
            //modelBuilder.Entity<WorkItem>()
            //    .Property(x => x.State)
            //    .IsRequired();

            //modelBuilder.Entity<WorkItem>()
            //    .Property(x => x.Area)
            //    .HasColumnType("varchar(200)");

            //or we can do this, like this in one modelBuilder
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.EndDate).HasPrecision(3);
                eb.Property(x => x.Efford).HasColumnType("decimal(5, 2)");
                eb.Property(x => x.Activity).HasMaxLength(200);
                eb.Property(x => x.RemaningWork).HasPrecision(14, 2);
                eb.Property(x => x.Priority).HasDefaultValue(3);

                //relations one-to-many  WorkItem--[1]----[*]--Comment
                eb.HasMany(wi => wi.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

                //relations many-to-one  WorkItem--[*]----[1]--User
                eb.HasOne(wi => wi.Author)
                .WithMany(u => u.WorkItems)
                .HasForeignKey(wi => wi.AuthorId);

                //relations many-to-many  WorkItem--[*]----[*]--Tag
                eb.HasMany(w => w.Tags)
                .WithMany(t => t.WorkItems)
                .UsingEntity<WorkItemTag>(

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
                eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(w => w.StateId);

            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreateDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            });

            //relations one-to-one  User--[1]----[1]--Address
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            //relations many-to-many  WorkItem--[*]----[*]--Tag
            //modelBuilder.Entity<WorkItemTag>()
            //    .HasKey(c => new { c.TagId, c.WorkItemId });


            modelBuilder.Entity<WorkItemState>()
                .Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(50);


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
