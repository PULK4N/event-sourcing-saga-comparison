using Contracts;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace EventSourcing.Core.Providers
{
    public class DemoReducerProvider : IReducerProvider
    {
        /*
         * Demo version of reducer provider. Uses AccountTranscriber reducer instead of reading from the configuration
         */
        public Task<IEventReducer> GetReducer(EventPayload payload)
        {
            var reducerName = "AccountTransactionReducer";
            var type = Type.GetType(reducerName);
            if (type is null)
                throw new ReducerNotFoundException(reducerName);

            var reducer = Activator.CreateInstance(type);

            if (reducer is not IEventReducer eventReducer)
                throw new ReducerNotRegisteredException(reducerName);

            return Task.FromResult(eventReducer);
        }
    }
}
