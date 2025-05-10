using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace InventoryModule.Events;

public class InvetoryItemsOrdered : IEvent
{
    public Guid OrderId { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var inventoryStateData = (InventoryStateData)stateData;
        inventoryStateData.ReservedInventoryItems.Remove(OrderId);

        return inventoryStateData;
    }
}
