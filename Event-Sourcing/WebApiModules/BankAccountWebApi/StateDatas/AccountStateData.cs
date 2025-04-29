using EventSourcing.Models;

namespace BankAccountWebApi.StateDatas;

public class AccountStateData : ISharedStateData
{
    public bool IsDeleted { get; set; }
    public float Money { get; set; }
}
