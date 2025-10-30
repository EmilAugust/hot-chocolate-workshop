using HotChocolate.Subscriptions;
using HotChocolateWorkshop.Entities;

namespace HotChocolateWorkshop.Graph;

public class Subscription
{
    [Subscribe(With = nameof(SubscribeToCreatedRockets))]
    public Rocket RocketCreated(
        [EventMessage] Rocket rocket) => rocket;

    private async IAsyncEnumerable<Rocket> SubscribeToCreatedRockets(
        ITopicEventReceiver topicEventReceiver,
        string? severity)
    {
       var subscription = await topicEventReceiver
           .SubscribeAsync<Rocket>(nameof(RocketCreated));

       await foreach (var rocket in subscription.ReadEventsAsync())
       {
           if (severity != null && rocket.Description != severity)
           {
               continue;
           }
           
           yield return rocket;
       }
    }
}