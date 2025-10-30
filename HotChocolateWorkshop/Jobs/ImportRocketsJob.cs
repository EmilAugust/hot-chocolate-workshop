using System.Text.Json;

namespace HotChocolateWorkshop.Jobs;

public class ImportRocketsJob(IHttpClientFactory httpClientFactory)
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
        
        Console.WriteLine($"Found {rockets.Length} rockets");
    }

    private class SpaceXRocketDto
    {
        public required string RocketName { get; init; }
        public required string Description { get; init; }
    }
}