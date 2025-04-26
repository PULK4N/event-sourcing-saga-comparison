using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IEventHook
    {
        Task RunHook(HookInfo hookInfo);
    }
}
