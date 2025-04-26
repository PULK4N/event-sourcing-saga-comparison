using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IHookProvider
    {
        // Task<Dictionary<EventPayload, List<IEventHook>>> GetHooksByEvents(
        //     IEnumerable<EventPayload> payloads
        // );
    }
}
