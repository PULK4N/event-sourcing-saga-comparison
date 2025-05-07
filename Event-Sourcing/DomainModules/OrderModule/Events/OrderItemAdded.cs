using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;
using OrderModule.Models;

namespace OrderModule.Events;

public class OrderItemAdded : IEvent
{
    public OrderItem OrderItem { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var orderStateData = (OrderStateData)stateData;
        orderStateData.Items.Add(OrderItem);

        return stateData;
    }
}
