using InventoryModule.Models;

namespace InventoryImplementationModule.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}
