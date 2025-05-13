using EventSourcing.Shared.Models;
using OrderModule.Models;

namespace OrderModule;

public class OrderStateData : ISharedStateData
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime CreationTimestamp { get; set; }
    public DateTime? TimeOfOrderPlacement { get; set; }
    public DateTime LastUpdateTimestamp { get; set; }
}
