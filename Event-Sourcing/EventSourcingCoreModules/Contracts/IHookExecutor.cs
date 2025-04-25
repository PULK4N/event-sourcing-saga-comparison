using EventSourcing.Models;

namespace Contracts
{
    public interface IHookExecutor
    {
        Task RegisterHooksForExecution(params EventPayload[] payloads);
    }
}
