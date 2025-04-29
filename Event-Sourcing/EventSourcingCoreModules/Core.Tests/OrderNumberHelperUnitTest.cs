using EventSourcing.Core.Providers;
using EventSourcing.Models;

namespace Core.Tests;

public class OrderNumberHelperUnitTest
{
    [Fact()]
    public void AssignOrderNumbersToNewEventsIfPreviousExist()
    {
        var payloads = new List<EventPayload>();

        for (uint i = 1; i <= 10; i++)
        {
            var payload = EventPayload.Create(
                new Dictionary<string, object>(),
                $"RandomEventName{i}",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "test-state-machine"
            );
            payload.OrderNumber = i;
            payloads.Add(payload);
        }

        var newPayloads = new List<EventPayload>();
        for (int i = 0; i < 3; i++)
        {
            var payload = EventPayload.Create(
                new Dictionary<string, object>(),
                $"NewEventName{i}",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "test-state-machine"
            );
            newPayloads.Add(payload);
        }

        var orderNumberHelper = new OrderNumberHelper();
        orderNumberHelper.AssignOrderNumbers(payloads, newPayloads);

        Assert.Equal(newPayloads[0].OrderNumber.ToString(), 11.ToString());
        Assert.Equal(newPayloads[1].OrderNumber.ToString(), 12.ToString());
        Assert.Equal(newPayloads[2].OrderNumber.ToString(), 13.ToString());
    }

    [Fact]
    public void AssignOrderNumbersToNewEventsIfThereAreNoPreviousEvents()
    {
        var payloads = new List<EventPayload>();

        var newPayloads = new List<EventPayload>();
        for (int i = 0; i < 3; i++)
        {
            var payload = EventPayload.Create(
                new Dictionary<string, object>(),
                $"NewEventName{i}",
                Guid.NewGuid(),
                Guid.NewGuid(),
                "test-state-machine"
            );
            newPayloads.Add(payload);
        }

        var orderNumberHelper = new OrderNumberHelper();
        orderNumberHelper.AssignOrderNumbers(payloads, newPayloads);

        Assert.Equal(newPayloads[0].OrderNumber.ToString(), 1.ToString());
        Assert.Equal(newPayloads[1].OrderNumber.ToString(), 2.ToString());
        Assert.Equal(newPayloads[2].OrderNumber.ToString(), 3.ToString());
    }
}
