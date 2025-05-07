using EventSourcing.Shared.Models;
using InventoryModule.Models;

namespace InventoryModule;

public class InventoryStateData : ISharedStateData
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<Guid, InventoryItem> InventoryItems { get; set; } =
        new Dictionary<Guid, InventoryItem>();
    public Dictionary<Guid, List<InventoryItem>> ReservedInventoryItems { get; set; } =
        new Dictionary<Guid, List<InventoryItem>>();
}
