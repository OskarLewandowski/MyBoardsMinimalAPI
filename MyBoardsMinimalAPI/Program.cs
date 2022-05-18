using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoardsMinimalAPI.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//fix json serialize cycle
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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

//Endpoints get
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

app.MapGet("selectedEpics", async (MyBoardsMinimalAPIContext db) =>
{
    var selectedEpics = await db.Epic
    .Where(w => w.StateId == 4)
    .OrderBy(w => w.Priority)
    .ToListAsync();

    return selectedEpics;
});

app.MapGet("userWhoHaveTheMostComment", async (MyBoardsMinimalAPIContext db) =>
{
    var authorsCommentCountsQuery = db.Comments
    .GroupBy(c => c.AuthorId)
    .Select(g => new { g.Key, Count = g.Count() });

    var authorsCommentCounts = await authorsCommentCountsQuery.ToListAsync();

    var topAuthor = authorsCommentCounts
    .First(a => a.Count == authorsCommentCounts
    .Max(acc => acc.Count));

    var userDetails = db.Users.First(u => u.Id == topAuthor.Key);

    return new { userDetails, commentCount = topAuthor.Count };
});

//Endpoints update
app.MapPost("update", async (MyBoardsMinimalAPIContext db) =>
{
    Epic epic = await db.Epic.FirstAsync(epic => epic.Id == 1);

    epic.Area = "Updated area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;
    epic.StateId = 1;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("update2", async (MyBoardsMinimalAPIContext db) =>
{
    Epic epic = await db.Epic.FirstAsync(epic => epic.Id == 1);

    var onHoldState = await db.WorkItemStates.FirstAsync(a => a.Value == "On Hold");

    epic.StateId = onHoldState.Id;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("update3", async (MyBoardsMinimalAPIContext db) =>
{
    Epic epic = await db.Epic.FirstAsync(epic => epic.Id == 1);

    var rejectedState = await db.WorkItemStates.FirstAsync(a => a.Value == "Rejected");

    epic.State = rejectedState;

    await db.SaveChangesAsync();

    return epic;
});

//Endpoints create
app.MapPost("create", async (MyBoardsMinimalAPIContext db) =>
{
    Tag tag = new Tag()
    {
        Value = "EF"
    };

    //await db.AddAsync(tag);
    await db.Tags.AddAsync(tag);
    await db.SaveChangesAsync();

    return tag;
});

app.MapPost("create2", async (MyBoardsMinimalAPIContext db) =>
{
    Tag tag1 = new Tag()
    {
        Value = "EF"
    };
    Tag tag2 = new Tag()
    {
        Value = "MVC"
    };
    Tag tag3 = new Tag()
    {
        Value = "ASP"
    };

    var tags = new List<Tag>() { tag1, tag2, tag3 };

    //await db.AddAsync(tag);
    //await db.Tags.AddAsync(tag1);
    //await db.Tags.AddAsync(tag2);
    //await db.Tags.AddAsync(tag3);
    //await db.Tags.AddRangeAsync(tag1,tag2,tag3);

    await db.Tags.AddRangeAsync(tags);
    await db.SaveChangesAsync();
});

app.MapPost("create3", async (MyBoardsMinimalAPIContext db) =>
{
    var address = new Address()
    {
        Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
        City = "Kraków",
        Country = "Poland",
        Street = "Krótka"
    };

    var user = new User()
    {
        Email = "userAdd@test.com",
        FullName = "Test User12",
        Address = address
    };

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
});

//Endpoints loading related data
app.MapGet("getAuthorComments", async (MyBoardsMinimalAPIContext db) =>
{
    //var user = await db.Users.FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));
    //var userComments = await db.Comments.Where(c => c.AuthorId == user.Id).ToListAsync();

    var userWithComments = await db.Users
    .Include(u => u.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    return userWithComments;
});

app.MapGet("getAuthorAllInformation", async (MyBoardsMinimalAPIContext db) =>
{
    var userWithDetails = await db.Users
    .Include(u => u.Comments)
    .ThenInclude(c => c.WorkItem)
    .Include(u => u.Address)
    .FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    return userWithDetails;
});

//Endpoints delete 
//ON DELETE CASCADE
app.MapDelete("deleteWorkItemTag", async (MyBoardsMinimalAPIContext db) =>
{
    var workItemTags = await db.WorkItemTag
        .Where(c => c.WorkItemId == 12)
        .ToListAsync();

    db.WorkItemTag.RemoveRange(workItemTags);
    //or
    //db.RemoveRange(workItemTags);

    var workItem = await db.WorkItems.FirstAsync(c => c.Id == 16);

    db.RemoveRange(workItem);
    await db.SaveChangesAsync();
});

app.MapDelete("deleteUser", async (MyBoardsMinimalAPIContext db) =>
{
    var user = await db.Users.FirstAsync(u => u.Id == Guid.Parse("DC231ACF-AD3C-445D-CC08-08DA10AB0E61"));

    var userComments = await db.Comments.Where(c => c.AuthorId == user.Id).ToListAsync();

    db.Comments.RemoveRange(userComments);
    //db.Remove(userComments);

    db.Users.Remove(user);

    await db.SaveChangesAsync();
});

app.MapDelete("deleteUserClientCascade", async (MyBoardsMinimalAPIContext db) =>
{
    var user = await db.Users
    .Include(u => u.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("4EBB526D-2196-41E1-CBDA-08DA10AB0E61"));

    db.Users.Remove(user);
    await db.SaveChangesAsync();
});


app.Run();
