using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Graph;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Incident>>();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.MapGraphQL();
app.MapNitroApp();

app.Run();