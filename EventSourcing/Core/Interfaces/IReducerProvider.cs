using Contracts;
using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface IReducerProvider
    {
        Task<IEventReducer> GetReducer(EventPayload payload);
    }
}
