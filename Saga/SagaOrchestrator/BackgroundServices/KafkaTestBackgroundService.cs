using CommunicationModule.Interfaces;

// 1. Pokrenuti kafku
// 2. Testirati da li radi slanje poruka sa Saga orchestratora
// 3. Poslati poruku account service
// 4. Pokrenuti account service
// 5. Dodati CommunicationModule u accountService
// 6. Dodati background service
// 7. Injecktovati background service
// 8. Primiti poruku account manageru
// 9. Dodati slanje poruka u account manageru
// 10. Promeniti u kafki da jedan queue bude saga return koji bi primao sve requestove od drugih mikroservisa
// 11.
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
            await _messageConsumer.ConsumeAsync(
                "testSendingDataFromSaga",
                async (kafkaTestModel, string2) =>
                {
                    Console.WriteLine(kafkaTestModel.ToString());
                    await Task.FromResult(kafkaTestModel);
                },
                stoppingToken
            );
        }
    }
}
