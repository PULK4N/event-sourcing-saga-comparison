using CommunicationModule.Interfaces;

namespace AccountManager.BackgroundServices
{
    public class KafkaMessageReceiverBackgroundService : BackgroundService
    {
        private readonly IMessageConsumer<string, string> _messageConsumer;
        private readonly IMessageProducer<string, string> _messageProducer;

        public KafkaMessageReceiverBackgroundService(
            IMessageConsumer<string, string> messageConsumer,
            IMessageProducer<string, string> messageProducer
        )
        {
            _messageConsumer = messageConsumer;
            _messageProducer = messageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync(
                "account-debit-request",
                async (kafkaTestModel, string2) =>
                {
                    Console.WriteLine(kafkaTestModel.ToString());
                    await _messageProducer.ProduceAsync(
                        "account-debit-response",
                        kafkaTestModel,
                        string2
                    );
                    await Task.FromResult(kafkaTestModel);
                },
                stoppingToken
            );
        }
    }
}
