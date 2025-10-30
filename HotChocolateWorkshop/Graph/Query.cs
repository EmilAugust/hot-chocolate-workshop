using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Persistence;

namespace HotChocolateWorkshop.Graph;

public class Query
{
    public IQueryable<Rocket> Rockets([Service] AppDbContext dbContext)
    {
        return dbContext.Rockets;
    }
}