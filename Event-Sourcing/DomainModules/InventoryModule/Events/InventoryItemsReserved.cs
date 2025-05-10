using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;
using InventoryModule.Models;

namespace InventoryModule.Events;

public class InvetoryItemsReserved : IEvent
{
    public Guid OrderId { get; set; }
    public List<InventoryItem> ReservedItems { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var inventoryStateData = (InventoryStateData)stateData;
        inventoryStateData.Id = eventExecutionInfo.AggregateId;
        inventoryStateData.CreationTimestamp = eventExecutionInfo.Timestamp;
        foreach (var inventoryItem in ReservedItems)
        {
            inventoryStateData.InventoryItems[inventoryItem.Id].Counter -= inventoryItem.Counter;

            if (NoItemsLeft(inventoryStateData, inventoryItem))
                inventoryStateData.InventoryItems.Remove(inventoryItem.Id);
        }

        inventoryStateData.ReservedInventoryItems[OrderId] = ReservedItems;

        return inventoryStateData;
    }

    private bool NoItemsLeft(InventoryStateData inventoryStateData, InventoryItem inventoryItem)
    {
        return inventoryStateData.InventoryItems[inventoryItem.Id].Counter == 0;
    }
}
