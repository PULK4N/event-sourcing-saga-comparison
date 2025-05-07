using EventSourcing.Shared.Models;

namespace PaymentModule;

public class PaymentStateData : ISharedStateData
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public Guid OrderId { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Metadata { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CompletitionTimestamp { get; set; }
}
