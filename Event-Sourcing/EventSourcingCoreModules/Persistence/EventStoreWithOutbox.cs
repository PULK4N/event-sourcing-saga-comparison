using EventSourcing.Core.Interfaces;
using EventSourcing.Persistence.Models;
using EventSourcing.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EventSourcing.Persistence;

public class EventStoreWithOutbox : IEventStoreWithOutbox
{
    private readonly EventSourcingDbContext _applicationDbContext;

    public EventStoreWithOutbox(EventSourcingDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Dictionary<Guid, EventPayload[]>> GetEventsByAggregate(
        params Guid[] AggregateIds
    )
    {
        var serializedPayloads = await _applicationDbContext
            .SerializedEventPayload
            .Where(x => AggregateIds.Contains(x.AggregateId))
            .AsNoTracking()
            .ToListAsync();

        var payloads = serializedPayloads.Select(
            x => JsonConvert.DeserializeObject<EventPayload>(x.SerializedJsonData)
        );

        var eventsDictionary = new Dictionary<Guid, EventPayload[]>();

        foreach (var aggregateId in AggregateIds)
        {
            var aggregateEvents = payloads.Where(
                x => x.EventExecutionInfo.AggregateId == aggregateId
            );
            eventsDictionary.Add(aggregateId, aggregateEvents.ToArray());
        }

        return eventsDictionary;
    }

    public async Task WriteEventsWithOutbox(params EventPayload[] payloads)
    {
        var aggregateIds = payloads.Select(x => x.EventExecutionInfo.AggregateId);
        var serializedPayloads = payloads.Select(SerializedEventPayload.FromPayload);

        var serializedPayloadMessages = payloads.Select(SerializedPayloadMessage.FromPayload);

        await _applicationDbContext
            .SerializedPayloadMessage
            .AddRangeAsync(serializedPayloadMessages);
        await _applicationDbContext.SerializedEventPayload.AddRangeAsync(serializedPayloads);
        await _applicationDbContext.SaveChangesAsync();
    }
}
