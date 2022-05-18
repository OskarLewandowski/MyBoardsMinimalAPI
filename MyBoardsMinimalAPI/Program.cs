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

//Endpoints
app.MapGet("Tags", (MyBoardsMinimalAPIContext db) =>
{
    var tags = db.Tags.ToList();
    return tags;
});

app.MapGet("Data", (MyBoardsMinimalAPIContext db) =>
{
    var epic = db.Epic.First();
    var user = db.Users.First(u => u.FullName == "User One");
    return new { epic, user };
});

app.MapGet("ToDO", (MyBoardsMinimalAPIContext db) =>
{
    var toDoWorkItems = db.WorkItems
    .Where(w => w.StateId == 1)
    .ToList();

    return toDoWorkItems;
});

//we have better performance where we use async
app.MapGet("Comments", async (MyBoardsMinimalAPIContext db) =>
{
    var newComments = await db.Comments
    .Where(c => c.CreatedDate > new DateTime(2022, 7, 23))
    .ToListAsync();

    return newComments;
});

app.MapGet("Top5NewestComments", async (MyBoardsMinimalAPIContext db) =>
{
    var top5NewestComments = await db.Comments
    .OrderByDescending(c => c.CreatedDate)
    .Take(5)
    .ToListAsync();

    return top5NewestComments;
});

app.MapGet("statesCount", async (MyBoardsMinimalAPIContext db) =>
{
    var statesCount = await db.WorkItems
    .GroupBy(x => x.StateId)
    .Select(g => new { stateId = g.Key, count = g.Count() })
    .ToListAsync();

    return statesCount;
});


app.Run();
