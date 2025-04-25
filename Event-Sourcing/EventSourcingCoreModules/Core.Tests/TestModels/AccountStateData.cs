using EventSourcing.Models;

namespace Core.Tests.TestModels;

public class AccountStateData : ISharedStateData
{
    public float Money { get; set; }
    public bool IsDeleted { get; set; }
}
