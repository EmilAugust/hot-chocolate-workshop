namespace HotChocolateWorkshop.Entities;

public class Rocket
{
    public required Guid Id { get; init; }
    public required string RocketId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; set; }
    
    public required List<Launch> Launches { get; init; }
}