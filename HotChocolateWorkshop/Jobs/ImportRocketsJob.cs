using System.Text.Json;
using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Persistence;

namespace HotChocolateWorkshop.Jobs;

public class ImportRocketsJob(IHttpClientFactory httpClientFactory, AppDbContext dbContext)
{
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
        
        dbContext.Rockets.AddRange(rockets.Select(rocket => new Rocket
        {
            Id = Guid.NewGuid(),
            Name = rocket.RocketName,
            Description = rocket.Description
        }));
        
        await dbContext.SaveChangesAsync();
        
        Console.WriteLine($"Found {rockets.Length} rockets");
    }

    private class SpaceXRocketDto
    {
        public required string RocketName { get; init; }
        public required string Description { get; init; }
    }
}