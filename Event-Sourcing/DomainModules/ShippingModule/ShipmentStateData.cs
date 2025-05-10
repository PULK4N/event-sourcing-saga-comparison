using EventSourcing.Shared.Models;

namespace ShippingModule;

public class ShipmentStateData : ISharedStateData
{
    public Guid Id { get; set; }
    public DateTime CreationTimestamp { get; set; }
    public DateTime LastUpdateTimestamp { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public Guid OrderId { get; set; }
}
