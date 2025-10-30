namespace HotChocolateWorkshop.Entities;

public class Incident
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Severity { get; init; }
}