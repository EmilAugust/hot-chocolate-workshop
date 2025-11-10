using HotChocolateWorkshop.Entities;
using HotChocolateWorkshop.Persistence;

namespace HotChocolateWorkshop.Graph;

public class Query
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<Rocket> Rockets([Service] AppDbContext dbContext)
    {
        return dbContext.Rockets;
    }
}