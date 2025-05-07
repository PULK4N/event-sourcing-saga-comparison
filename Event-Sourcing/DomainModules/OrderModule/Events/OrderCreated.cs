using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace OrderModule.Events;

public class OrderCreated : IEvent
{
    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var orderStateData = (OrderStateData)stateData;
        orderStateData.Id = eventExecutionInfo.AggregateId;
        orderStateData.CreationTimestamp = eventExecutionInfo.Timestamp;
        return stateData;
    }
}
