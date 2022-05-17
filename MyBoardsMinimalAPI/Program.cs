using Microsoft.EntityFrameworkCore;
using MyBoardsMinimalAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register in Di and connection to Db
builder.Services.AddDbContext<MyBoardsMinimalAPIContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsMinimalAPIContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();

if (!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@one.com",
        FullName = "User One",
        Address = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };

    var user2 = new User()
    {
        Email = "user2@two.com",
        FullName = "User Two",
        Address = new Address()
        {
            City = "Kraków",
            Street = "D³uga"
        }
    };

    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

//task data seed tags example 1 of 2
var tags = dbContext.Tags.ToList();
if (!tags.Any())
{
    var tag1 = new Tag()
    {
        Value = "Web"
    };
    var tag2 = new Tag()
    {
        Value = "UI"
    };
    var tag3 = new Tag()
    {
        Value = "Desktop"
    };
    var tag4 = new Tag()
    {
        Value = "API"
    };
    var tag5 = new Tag()
    {
        Value = "Service"
    };

    dbContext.Tags.AddRange(tag1, tag2, tag3, tag4, tag5);
    dbContext.SaveChanges();
}

app.Run();
