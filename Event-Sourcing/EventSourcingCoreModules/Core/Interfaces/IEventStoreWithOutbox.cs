using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IEventStoreWithOutbox
    {
        Task<Dictionary<Guid, EventPayload[]>> GetEventsByAggregate(params Guid[] AggregateId);

        Task WriteEventsWithOutbox(params EventPayload[] payloads);
    }
}
