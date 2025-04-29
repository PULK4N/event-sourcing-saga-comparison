using EventSourcing.Core;
using EventSourcing.Models;

namespace BankAccountWebApi.Commands;

public class SendMoneyCommand
{
    public float MoneyToTransfer { get; set; }
    public Guid CurrentUser { get; set; }
    public Guid UserSentTo { get; set; }
    private readonly StateMachineHandler _stateMachineHandler;

    public SendMoneyCommand(StateMachineHandler stateMachineHandler)
    {
        _stateMachineHandler = stateMachineHandler;
    }

    public async Task<object> ExecuteInternal()
    {
        var eventData = new Dictionary<string, object>()
        {
            { "moneyTransfered", -MoneyToTransfer }
        };
        var payload = EventPayload.Create(
            eventData,
            "TransferMoney",
            Guid.NewGuid(),
            "test-state-machine"
        );

        var receivePayloadData = new Dictionary<string, object>()
        {
            { "moneyTransfered", MoneyToTransfer }
        };
        var receivePayload = EventPayload.Create(
            receivePayloadData,
            "MoneyReceivedByTransfer",
            Guid.NewGuid(),
            "test-state-machine"
        );

        var result = await _stateMachineHandler.ExecuteEvents(payload, receivePayload);

        return result;
    }
}
