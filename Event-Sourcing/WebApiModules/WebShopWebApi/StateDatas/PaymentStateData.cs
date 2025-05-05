using EventSourcing.Shared.Models;

namespace WebShopWebApi.StateDatas
{
    public class PaymentStateData : ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
