using Contracts;
using EventSourcing.Core.Containers;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.Core
{
    public static class Registration
    {
        public static IServiceCollection RegisterInjection(this ServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environmentName == "development")
                RegisterDevelopmentEnvironment(services);
            else
                RegisterProductionEnvironment(services);

            services.RegisterReducers();

            return services;
        }

        private static void RegisterProductionEnvironment(ServiceCollection services)
        {
            throw new NotImplementedException();
        }

        private static void RegisterDevelopmentEnvironment(ServiceCollection services)
        {
            throw new NotImplementedException();
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
    }
}
