using Contracts;
using EventSourcing.Core.Containers;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace EventSourcing.Core.Providers
{
    public class ReducerProvider : IReducerProvider
    {
        public Task<IEventReducer> GetReducer(EventPayload payload)
        {
            throw new NotImplementedException();

            // Change the way this is implemented
            var reducerName = string.Empty;

            var type = ReducerTypeContainer.GetReducerType(reducerName);
            if (type is null)
                throw new ReducerNotFoundException(reducerName);

            var reducer = Activator.CreateInstance(type);

            if (reducer is not IEventReducer eventReducer)
                throw new ReducerNotRegisteredException(reducerName);

            return Task.FromResult(eventReducer);
        }
    }
}
