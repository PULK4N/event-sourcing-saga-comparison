using BankAccountWebApi.Models;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BankAccountWebApi.EventSourcing;

public class EventStoreWithOutbox : IEventStoreWithOutbox
{
    private readonly EventSourcingDbContext _applicationDbContext;

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
            var aggregateEvents = payloads.Where(x => x.AggregateId == aggregateId);
            eventsDictionary.Add(aggregateId, aggregateEvents.ToArray());
        }

        return eventsDictionary;
    }

    public async Task WriteEventsWithOutbox(params EventPayload[] payloads)
    {
        var aggregateIds = payloads.Select(x => x.AggregateId);
        var serializedPalyads = payloads.Select(SerializedEventPayload.FromPayload);

        var dbpayloads = _applicationDbContext
            .SerializedEventPayload
            .Where(x => aggregateIds.Contains(x.AggregateId));

        _applicationDbContext.SerializedEventPayload.RemoveRange(dbpayloads);

        var allPayloads = dbpayloads.ToList();
        allPayloads.AddRange(serializedPalyads);

        var serializedPayloadMessages = payloads.Select(SerializedPayloadMessage.FromPayload);

        await _applicationDbContext
            .SerializedPayloadMessage
            .AddRangeAsync(serializedPayloadMessages);
        await _applicationDbContext.SerializedEventPayload.AddRangeAsync(allPayloads);
        await _applicationDbContext.SaveChangesAsync();
    }
}
