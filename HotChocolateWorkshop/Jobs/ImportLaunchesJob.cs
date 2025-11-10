using System.Text.Json;
using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotChocolateWorkshop.Jobs;

public class ImportLaunchesJob(
    IHttpClientFactory httpClientFactory,
    AppDbContext dbContext)
{
    public const string JobName = "Import Launches";
    
    private const string Url = "https://api.spacexdata.com/v3/launches";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    public async Task RunAsync()
    {
        var httpClient = httpClientFactory.CreateClient();

        var launches = await httpClient
                          .GetFromJsonAsync<SpaceXLaunchDto[]>(Url, SerializerOptions)
                      ?? throw new InvalidOperationException("Invalid launch response.");

        foreach (var launch in launches)
        {
            var dbRocket = await dbContext.Rockets
                .Include(i => i.Launches)
                .FirstOrDefaultAsync(dbRocket => dbRocket.RocketId == launch.Rocket.RocketId);
            
            if (dbRocket is null)
                continue;
            
            dbRocket.Launches.Add(new Launch
            {
                Id = Guid.NewGuid(),
                Details = launch.Details ?? "",
                LaunchYear = launch.LaunchYear,
            });
        }
        
        await dbContext.SaveChangesAsync();
    }

    private class SpaceXLaunchDto
    {
        public required string LaunchYear { get; init; }
        public required string? Details { get; init; }
        public required RocketMapping Rocket { get; init; }
        
        public class RocketMapping
        {
            public required string RocketId { get; init; }
        }
    }
}