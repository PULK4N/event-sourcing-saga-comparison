using Contracts;
using EventSourcing.Core.Containers;
using EventSourcing.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.Core
{
    public static class Registration
    {
        public static IServiceCollection RegisterInjection(this ServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            services.RegisterReducers();

            return services;
        }

        public static ServiceCollection RegisterReducers(this ServiceCollection services)
        {
            var interfaceType = typeof(IEventReducer);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var allImplementations = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type))
                .Where(type => !type.IsInterface)
                .Where(type => !type.IsAbstract);

            foreach (var implementation in allImplementations)
            {
                if (implementation is Type type)
                {
                    ReducerTypeContainer.AddReducerType(implementation.ToString(), type);
                }
                services.AddTransient(implementation);
            }

            return services;
        }

        public static ServiceCollection RegisterStateDataTypes(this ServiceCollection services)
        {
            var interfaceType = typeof(ISharedStateData);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var allImplementations = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type))
                .Where(type => !type.IsInterface)
                .Where(type => !type.IsAbstract);

            foreach (var implementation in allImplementations)
            {
                if (implementation is Type type)
                    StateDataTypeContainer.AddStateDataType(implementation.ToString(), type);
            }

            return services;
        }
    }
}
