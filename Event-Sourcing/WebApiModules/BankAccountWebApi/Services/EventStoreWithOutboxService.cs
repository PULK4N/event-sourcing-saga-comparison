using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace BankAccountWebApi.Controllers;

public class EventStoreWithOutboxService : IEventStoreWithOutbox
{
    public Task<Dictionary<Guid, EventPayload[]>> GetEventsByAggregate(params Guid[] AggregateId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EventPayload>> WriteEventsWithOutbox(params EventPayload[] payloads)
    {
        throw new NotImplementedException();
    }
}
