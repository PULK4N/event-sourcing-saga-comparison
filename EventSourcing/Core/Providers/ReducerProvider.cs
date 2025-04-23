using Contracts;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace EventSourcing.Core.Providers
{
    public class ReducerProvider : IReducerProvider
    {
        public Task<IEventReducer> GetReducer(EventPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
