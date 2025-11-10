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
        string rocketId,
        string name,
        string description)
    {
        var rocket = new Rocket
        {
            Id = Guid.NewGuid(),
            RocketId = rocketId,
            Name = name,
            Description = description,
            Launches = []
        };

        dbContext.Rockets.Add(rocket);
        await dbContext.SaveChangesAsync();
        
        await topicEventSender.SendAsync(
            nameof(Subscription.RocketCreated),
            rocket);
        
        return rocket;
    }

    public string ImportRockets(
        IBackgroundJobClientV2 backgroundJobClient)
    {
        backgroundJobClient.Enqueue<ImportRocketsJob>(
            i => i.RunAsync());

        return "Job queued";
    }
    
    public string ScheduleImportRockets(
        string cron,
        IRecurringJobManagerV2 recurringJobManager)
    {
        recurringJobManager.AddOrUpdate<ImportRocketsJob>(
            ImportRocketsJob.JobName,
            i => i.RunAsync(),
            cron);

        return "Job scheduled";
    }
    
    public string ImportLaunches(
        IBackgroundJobClientV2 backgroundJobClient)
    {
        backgroundJobClient.Enqueue<ImportLaunchesJob>(
            i => i.RunAsync());

        return "Job queued";
    }
}