using Contracts;
using EventSourcing.Core.Containers;
using EventSourcing.Models;
using Microsoft.Extensions.Configuration;

namespace EventSourcing.Core.Providers
{
    public class AppSettingsConfigurationHookProvider : IHookProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public AppSettingsConfigurationHookProvider(
            IConfiguration configuration,
            IServiceProvider serviceProvider
        )
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Task<Dictionary<EventPayload, List<IEventHook>>> GetHooksByEvents(
            IEnumerable<EventPayload> payloads
        )
        {
            var payloadsHooks = new Dictionary<EventPayload, List<IEventHook>>();

            foreach (var payload in payloads)
            {
                var payloadHooks = GetEventHooksByPayload(payload);
                payloadsHooks.Add(payload, payloadHooks);
            }

            return Task.FromResult(payloadsHooks);
        }

        private List<IEventHook> GetEventHooksByPayload(EventPayload payload)
        {
            var hooks = new List<IEventHook>();

            var hookNames = _configuration[payload.EventName];

            if (string.IsNullOrEmpty(hookNames))
                return hooks;

            foreach (var hookName in hookNames.Split(';'))
            {
                var type = HookTypeContainer.GetHookType(hookName);
                if (type is null)
                    throw new HookTypeNotRegisteredException(hookName);

                var hook = _serviceProvider.GetService(type);

                if (hook is not IEventHook eventHook)
                    throw new HookTypeNotFoundException(hookName);

                hooks.Add(eventHook);
            }

            return hooks;
        }
    }
}
