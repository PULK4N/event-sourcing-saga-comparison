using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace PaymentModule.Events;

public class PaymentInitiated : IEvent
{
    public Guid OrderId { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
    public string Metadata { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var paymentStateData = (PaymentStateData)stateData;

        paymentStateData.Id = eventExecutionInfo.AggregateId;
        paymentStateData.Amount = Amount;
        paymentStateData.Currency = Currency;
        paymentStateData.Metadata = Metadata;
        paymentStateData.OrderId = OrderId;
        paymentStateData.Status = eventExecutionInfo.NewState;

        return paymentStateData;
    }
}
