using EventSourcing.Core.Containers;
using EventSourcing.Core.Interfaces;
using EventSourcing.Core.Providers;
using EventSourcing.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.Core
{
    public static class Registration
    {
        public static IServiceCollection RegisterInjection(this IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            services.AddScoped<OrderNumberHelper>();
            services.AddScoped<StateMachineHandler>();

            services.RegisterReducers();
            services.RegisterStateDataTypes();
            // services.RegisterHookTypes();

            if (environmentName == "development")
                services.RegisterDevEnvironmentProviders();
            else
                services.RegisterProdEnvironmentProviders();

            return services;
        }

        public static IServiceCollection RegisterDevEnvironmentProviders(
            this IServiceCollection services
        )
        {
            services.AddScoped<IStateDataProvider, AppSettingsConfigurationStateDataProvider>();
            services.AddScoped<IReducerProvider, AppSettingsConfigurationReducerProvider>();
            // services.AddScoped<IHookProvider, AppSettingsConfigurationHookProvider>();

            return services;
        }

        public static IServiceCollection RegisterProdEnvironmentProviders(
            this IServiceCollection services
        )
        {
            services.AddScoped<IStateDataProvider, StateDataProvider>();
            services.AddScoped<IReducerProvider, ReducerProvider>();

            return services;
        }

        public static IServiceCollection RegisterReducers(this IServiceCollection services)
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

        public static IServiceCollection RegisterStateDataTypes(this IServiceCollection services)
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

        // public static ServiceCollection RegisterHookTypes(this ServiceCollection services)
        // {
        //     var interfaceType = typeof(IEventHook);
        //     var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //
        //     var allImplementations = assemblies
        //         .SelectMany(a => a.GetTypes())
        //         .Where(type => interfaceType.IsAssignableFrom(type))
        //         .Where(type => !type.IsInterface)
        //         .Where(type => !type.IsAbstract);
        //
        //     foreach (var implementation in allImplementations)
        //     {
        //         if (implementation is Type type)
        //             HookTypeContainer.AddHookType(implementation.ToString(), type);
        //
        //         services.AddScoped(implementation);
        //     }
        //
        //     return services;
        // }
    }
}
