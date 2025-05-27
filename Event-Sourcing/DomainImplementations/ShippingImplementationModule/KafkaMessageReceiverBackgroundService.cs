using System.Text;
using System.Text.Json;
using CommunicationModule.Interfaces;
using ShippingModule;

namespace ShippingImplementationModule
{
    public class KafkaMessageReceiverBackgroundService : BackgroundService
    {
        private readonly IMessageConsumer<string, string> _messageConsumer;
        private readonly IServiceProvider _serviceProvider;

        public KafkaMessageReceiverBackgroundService(
            IMessageConsumer<string, string> messageConsumer,
            IServiceProvider serviceProvider
        )
        {
            _messageConsumer = messageConsumer;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync(
                "inventory-state-machine",
                async (key, value) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetService<ShipmentRepository>();
                    if (repository is null)
                        return;
                    byte[] byteArray = Encoding.UTF8.GetBytes(value);
                    var stream = new MemoryStream(byteArray);
                    var item = await JsonSerializer.DeserializeAsync<ShipmentStateData>(stream);
                    await repository.WriteDetails(item);
                },
                stoppingToken
            );
        }
    }
}
