using HotChocolateWorkshop.Entities;

namespace HotChocolateWorkshop.Graph;

public class Query
{
    public List<Incident> Incidents(
        [Service] List<Incident> incidents) => incidents;
}