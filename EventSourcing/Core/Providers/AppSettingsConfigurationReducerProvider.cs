using Contracts;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;
using Microsoft.Extensions.Configuration;

namespace EventSourcing.Core.Providers
{
    public class AppSettingsConfigurationReducerProvider : IReducerProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsConfigurationReducerProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<IEventReducer> GetReducer(EventPayload payload)
        {
            var reducerName = _configuration["EventReducerMap:" + payload.EventName];
            if (reducerName is null)
                throw new EventReducerMapNotAddedException(payload.EventName);

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
