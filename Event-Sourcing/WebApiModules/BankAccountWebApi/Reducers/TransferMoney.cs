using BankAccountWebApi.StateDatas;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace BankAccountWebApi.Reducers;

public class TransferMoney : IEventReducer
{
    public object Reduce(object stateData, EventPayload payload)
    {
        var moneyTransfered = float.Parse(payload.Data["moneyTransfered"].ToString());
        var accountStateaData = (AccountStateData)stateData;
        accountStateaData.Money -= moneyTransfered;

        return accountStateaData;
    }
}
