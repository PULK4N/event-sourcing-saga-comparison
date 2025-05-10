using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace OrderModule.Events;

public class OrderPlaced : IEvent
{
    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var orderStateData = (OrderStateData)stateData;

        return stateData;
    }
}
