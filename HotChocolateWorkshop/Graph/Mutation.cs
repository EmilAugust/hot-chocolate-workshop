using Hangfire;
using HotChocolate.Subscriptions;
using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Jobs;
using HotChocolateWorkshop.Persistence;

namespace HotChocolateWorkshop.Graph;

public class Mutation
{
    public async Task<Rocket> CreateRocket(
        AppDbContext dbContext,
        ITopicEventSender topicEventSender,
        string name, string description)
    {
        var rocket = new Rocket
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };

        dbContext.Rockets.Add(rocket);
        await dbContext.SaveChangesAsync();
        
        await topicEventSender.SendAsync(
            nameof(Subscription.RocketCreated),
            rocket);
        
        return rocket;
    }

    public string ImportRockets(
        [Service] IBackgroundJobClientV2 backgroundJobClient)
    {
        backgroundJobClient.Enqueue<ImportRocketsJob>(
            i => i.RunAsync());

        return "Job queued";
    }
}