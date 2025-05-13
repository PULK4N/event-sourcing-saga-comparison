using ActionImplementations;
using InventoryModule.Models;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Commands.Inventory;

public class CreateOrder : Command<InventoryDTO>
{
    public List<InventoryItem> InventoryItems { get; set; }
}
