using EventSourcing.Shared.Models;

namespace WebShopWebApi.StateDatas
{
    public class InventoryItemStateData : ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
