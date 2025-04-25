using EventSourcing.Models;

namespace Contracts
{
    public interface IHookProvider
    {
        Task<Dictionary<EventPayload, List<IEventHook>>> GetHooksByEvents(
            IEnumerable<EventPayload> payloads
        );
    }
}
