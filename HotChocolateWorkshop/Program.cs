using Hangfire;
using Hangfire.MemoryStorage;
using HotChocolateWorkshop.Graph;
using HotChocolateWorkshop.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddProjections()
    .AddInMemorySubscriptions();

builder.Services.AddHangfire(option =>
{
    option.UseMemoryStorage();
});

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=app.db");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapGraphQL();
app.MapNitroApp();

app.MapHangfireDashboard();

app.Run();