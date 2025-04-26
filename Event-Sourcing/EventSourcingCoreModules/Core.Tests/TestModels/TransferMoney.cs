using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace Core.Tests.TestModels;

public class TransferMoney : IEventReducer
{
    public object Reduce(object stateData, EventPayload payload)
    {
        var accountStateData = (AccountStateData)stateData;

        var moneyToSubtract = Convert.ToSingle(payload.Data["moneySent"]);

        accountStateData.Money -= moneyToSubtract;

        return accountStateData;
    }
}
