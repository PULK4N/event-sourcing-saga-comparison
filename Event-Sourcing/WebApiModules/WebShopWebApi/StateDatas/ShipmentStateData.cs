using EventSourcing.Models;

namespace WebShopWebApi.StateDatas
{
    public class ShipmentStateData : ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
