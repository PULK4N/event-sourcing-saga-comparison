using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;
using InventoryModule.Models;

namespace InventoryModule.Events;

public class InvetoryItemsAdded : IEvent
{
    public List<InventoryItem> InventoryItems { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var inventoryStateData = (InventoryStateData)stateData;
        inventoryStateData.Id = eventExecutionInfo.AggregateId;
        inventoryStateData.CreationTimestamp = eventExecutionInfo.Timestamp;
        foreach (var inventoryItem in InventoryItems)
        {
            if (InventoryContainsItem(inventoryStateData, inventoryItem))
                inventoryStateData.InventoryItems.Add(inventoryItem.Id, inventoryItem);
            else
                inventoryStateData.InventoryItems[inventoryItem.Id].Counter += inventoryItem.Counter;
        }

        return inventoryStateData;
    }

    private bool InventoryContainsItem(
        InventoryStateData inventoryStateData,
        InventoryItem inventoryItem
    )
    {
        return inventoryStateData.InventoryItems.ContainsKey(inventoryItem.Id);
    }
}
