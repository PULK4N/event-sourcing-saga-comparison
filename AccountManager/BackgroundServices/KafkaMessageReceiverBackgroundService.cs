using CommunicationModule.Interfaces;

namespace AccountManager.BackgroundServices
{
    public class KafkaMessageReceiverBackgroundService : BackgroundService
    {
        private readonly IMessageConsumer<string, string> _messageConsumer;

        public KafkaMessageReceiverBackgroundService(
            IMessageConsumer<string, string> messageConsumer
        )
        {
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync(
                "account-debit-request",
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
