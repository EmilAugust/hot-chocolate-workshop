using HotChocolate.Subscriptions;
using HotChocolateWorkshop.Entities;

namespace HotChocolateWorkshop.Graph;

public class Subscription
{
    [Subscribe(With = nameof(SubscribeToCreatedIncidents))]
    public Incident IncidentCreated(
        [EventMessage] Incident incident) => incident;

    private async IAsyncEnumerable<Incident> SubscribeToCreatedIncidents(
        ITopicEventReceiver topicEventReceiver,
        string? severity)
    {
       var subscription = await topicEventReceiver
           .SubscribeAsync<Incident>(nameof(IncidentCreated));

       await foreach (var incident in subscription.ReadEventsAsync())
       {
           if (severity != null && incident.Severity != severity)
           {
               continue;
           }
           
           yield return incident;
       }
    }
}