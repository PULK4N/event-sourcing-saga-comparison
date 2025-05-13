using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace InventoryModule.Events;

public class InvetoryCreated : IEvent
{
    public string Name { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var inventoryStateData = (InventoryStateData)stateData;
        inventoryStateData.Id = eventExecutionInfo.AggregateId;
        inventoryStateData.CreationTimestamp = eventExecutionInfo.Timestamp;
        inventoryStateData.Name = Name;

        return inventoryStateData;
    }
}
