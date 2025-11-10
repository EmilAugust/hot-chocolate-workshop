using System.ComponentModel.DataAnnotations.Schema;

namespace HotChocolateWorkshop.Entities;

public class Launch
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; init; }
    
    public required string LaunchYear { get; init; }
    
    public required string Details { get; init; }
}