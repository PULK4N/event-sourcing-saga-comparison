using CommunicationModule.Interfaces;
using Newtonsoft.Json.Linq;
using SagaOrchestrator.Models;

namespace SagaOrchestrator
{
    public class KafkaTestBackgroundService : BackgroundService
    {
        private readonly IMessageConsumer<string, string> _messageConsumer;
        public KafkaTestBackgroundService(IMessageConsumer<string, string> messageConsumer)
        {
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync("testSendingDataFromSaga", async (kafkaTestModel, string2) =>
            {
                Console.WriteLine(kafkaTestModel.ToString());
                await Task.FromResult(kafkaTestModel);
            }, stoppingToken);
        }
    }
}
