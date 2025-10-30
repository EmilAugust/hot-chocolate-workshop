using HotChocolate.Subscriptions;
using HotChocolateWorkshop.Entities;

namespace HotChocolateWorkshop.Graph;

public class Mutation
{
    public async Task<Incident> CreateIncident(
        [Service] List<Incident> incidents,
        [Service] ITopicEventSender topicEventSender,
        string title, string severity)
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            Title = title,
            Severity = severity
        };
        
        incidents.Add(incident);
        
        await topicEventSender.SendAsync(
            nameof(Subscription.IncidentCreated),
            incident);
        
        return incident;
    }
}