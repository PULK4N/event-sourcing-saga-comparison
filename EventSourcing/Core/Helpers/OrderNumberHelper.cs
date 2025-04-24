using EventSourcing.Models;

namespace EventSourcing.Core.Providers
{
    public class OrderNumberHelper
    {
        public void AssignOrderNumbers(
            IEnumerable<EventPayload> existingEvents,
            IEnumerable<EventPayload> aggregateEventsToExecute
        )
        {
            uint currentLastOrderNumber = 0;
            if (existingEvents.Any())
                currentLastOrderNumber = existingEvents.Max(x => x.OrderNumber);

            foreach (var payload in aggregateEventsToExecute)
            {
                payload.OrderNumber = ++currentLastOrderNumber;
            }
        }
    }
}
