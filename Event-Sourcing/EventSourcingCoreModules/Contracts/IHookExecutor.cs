using EventSourcing.Models;

namespace Contracts
{
    public interface IHookExecutor
    {
        Task RunHook(HookInfo hookInfo);
    }
}
