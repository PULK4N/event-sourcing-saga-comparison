using ActionImplementations;
using InventoryModule.Models;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Commands.Inventory;

public class CreateInventory : Command<InventoryDTO>
{
    public string Name { get; set; }
    public List<InventoryItem> InventoryItems { get; set; }
}
