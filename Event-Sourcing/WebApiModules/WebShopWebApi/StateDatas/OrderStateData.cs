using EventSourcing.Shared.Models;

namespace WebShopWebApi.StateDatas
{
    public class OrderStateData : ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
