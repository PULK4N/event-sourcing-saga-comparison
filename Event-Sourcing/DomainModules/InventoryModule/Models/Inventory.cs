namespace InventoryModule.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public List<InventoryItem> Items { get; set; }
}
