using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace ShippingModule.Events;

public class ShipmentTransfered : IEvent
{
    public string Location { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var shipmentStateData = (ShipmentStateData)stateData;
        shipmentStateData.LastUpdateTimestamp = eventExecutionInfo.Timestamp;
        shipmentStateData.Status = eventExecutionInfo.NewState;
        shipmentStateData.Location = Location;

        return shipmentStateData;
    }
}
