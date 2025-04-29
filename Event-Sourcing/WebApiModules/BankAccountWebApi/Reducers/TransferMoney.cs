using BankAccountWebApi.StateDatas;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace BankAccountWebApi.Reducers;

public class TransferMoney : IEventReducer
{
    public object Reduce(object stateData, EventPayload payload)
    {
        var moneyTransfered = (float)payload.Data["moneyTransfered"];
        var accountStateaData = (AccountStateData)stateData;
        accountStateaData.Money -= moneyTransfered;

        return accountStateaData;
    }
}
