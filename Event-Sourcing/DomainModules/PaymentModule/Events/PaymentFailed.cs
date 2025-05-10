using EventSourcing.Shared.Interfaces;
using EventSourcing.Shared.Models;

namespace PaymentModule.Events;

public class PaymentFailed : IEvent
{
    public string Reason { get; set; }
    public string Metadata { get; set; }

    public object Apply(object stateData, EventExecutionInfo eventExecutionInfo)
    {
        var paymentStateData = (PaymentStateData)stateData;

        paymentStateData.Status = eventExecutionInfo.NewState;
        paymentStateData.CompletitionTimestamp = eventExecutionInfo.Timestamp;

        return paymentStateData;
    }
}
