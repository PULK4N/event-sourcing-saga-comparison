using CommunicationModule;
using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using Newtonsoft.Json.Linq;
using SagaOrchestrator.Models;

namespace SagaOrchestrator
{
    public static class RegisterInjection
    {
        public static IServiceCollection RegisterKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConsumerConfig>(configuration.GetSection("KafkaConsumerConfig"));
            services.Configure<KafkaProducerConfig>(configuration.GetSection("KafkaProducerConfig"));
            services.AddSingleton<IMessageProducer<string, string>, KafkaProducer<string, string>>();
            services.AddSingleton<IMessageConsumer<string, string>, KafkaConsumer<string, string>>();
            return services;
        }

    }
}
