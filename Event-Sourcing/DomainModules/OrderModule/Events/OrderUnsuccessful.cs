using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace OrderModule.Events;

public class OrderUnsuccessful : IEvent
{
    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var orderStateData = (OrderStateData)stateData;
        orderStateData.TimeOfOrderPlacement = eventExecutionInfo.Timestamp;

        return stateData;
    }
}
