using System.Text.Json;
using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotChocolateWorkshop.Jobs;

public class ImportRocketsJob(IHttpClientFactory httpClientFactory, AppDbContext dbContext)
{
    public const string JobName = "Import Rockets";
    
    private const string Url = "https://api.spacexdata.com/v3/rockets";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    public async Task RunAsync()
    {
        var httpClient = httpClientFactory.CreateClient();

        var rockets = await httpClient
                          .GetFromJsonAsync<SpaceXRocketDto[]>(Url, SerializerOptions)
                      ?? throw new InvalidOperationException("Invalid rocket response.");

        foreach (var rocket in rockets)
        {
            var dbRocket = await dbContext.Rockets
                .FirstOrDefaultAsync(dbRocket => dbRocket.Name == rocket.RocketName);
            
            if (dbRocket is not null)
            {
                if (dbRocket.Description != rocket.Description)
                {
                    dbRocket.Description = rocket.Description;
                }
                
                continue;
            }
            
            dbContext.Rockets.Add(new Rocket
            {
                Id = Guid.NewGuid(),
                RocketId = rocket.RocketId,
                Name = rocket.RocketName,
                Description = rocket.Description,
                Launches = []
            });
        }
        
        await dbContext.SaveChangesAsync();
    }

    private class SpaceXRocketDto
    {
        public required string RocketId { get; init; }
        public required string RocketName { get; init; }
        public required string Description { get; init; }
    }
}