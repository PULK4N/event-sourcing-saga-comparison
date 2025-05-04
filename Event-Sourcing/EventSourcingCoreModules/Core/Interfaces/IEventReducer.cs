using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IEventReducer
    {
        object Reduce(object stateData, EventPayload payload);
    }
}
