using InventoryModule.Models;

namespace WebShopWebApi.DTOs
{
    public class InventoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}
