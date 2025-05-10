using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace ShippingModule.Events;

public class ShipmentArrived : IEvent
{
    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var shipmentStateData = (ShipmentStateData)stateData;
        shipmentStateData.LastUpdateTimestamp = eventExecutionInfo.Timestamp;
        shipmentStateData.Status = eventExecutionInfo.NewState;

        return shipmentStateData;
    }
}
