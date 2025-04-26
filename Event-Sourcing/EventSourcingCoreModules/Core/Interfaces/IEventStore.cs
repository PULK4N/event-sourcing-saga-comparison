using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IEventStore
    {
        Task<Dictionary<Guid, EventPayload[]>> GetEventsByAggregate(params Guid[] AggregateId);

        Task<IEnumerable<EventPayload>> WriteEvents(params EventPayload[] payloads);
    }
}
