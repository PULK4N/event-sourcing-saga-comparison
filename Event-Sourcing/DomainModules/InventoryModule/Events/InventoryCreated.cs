using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace InventoryModule.Events;

public class InvetoryCreated : IEvent
{
    public Guid OrderId { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var inventoryStateData = (InventoryStateData)stateData;
        inventoryStateData.Id = eventExecutionInfo.AggregateId;
        inventoryStateData.CreationTimestamp = eventExecutionInfo.Timestamp;

        return inventoryStateData;
    }
}
