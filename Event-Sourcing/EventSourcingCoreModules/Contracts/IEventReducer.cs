using EventSourcing.Models;

namespace Contracts
{
    public interface IEventReducer
    {
        object Reduce(object stateData, EventPayload payload);
    }
}
