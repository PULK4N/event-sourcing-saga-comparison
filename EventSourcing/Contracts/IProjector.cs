using EventSourcing.Models;

namespace Contracts
{
    public interface IProjector : IEventHook
    {
        Task RunMultipleProjections(IEnumerable<HookInfo> hookInfo);
    }
}
