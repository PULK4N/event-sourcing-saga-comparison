using EventSourcing.Models;

namespace Contracts
{
    public interface IEventHook
    {
        Task RunHook(HookInfo stateInfo);
    }
}
