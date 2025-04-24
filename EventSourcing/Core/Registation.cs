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
    }
}
