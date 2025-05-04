namespace InventoryModule.Models;

public class InventoryItem
{
    public Guid Id { get; set; }
    public uint Counter { get; set; }
    public string Name { get; set; }
}
