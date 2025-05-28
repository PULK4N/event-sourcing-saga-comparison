using System.Text;
using System.Text.Json;
using CommunicationModule.Interfaces;
using InventoryImplementationModule.Models;
using InventoryModule;

namespace InventoryImplementationModule
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
                    var repository = scope.ServiceProvider.GetService<InventoryRepository>();
                    if (repository is null)
                        return;
                    byte[] byteArray = Encoding.UTF8.GetBytes(value);
                    var stream = new MemoryStream(byteArray);
                    var item = await JsonSerializer.DeserializeAsync<InventoryStateData>(stream);
                    if (item is null)
                        throw new NullReferenceException();
                    var inventory = new Inventory()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        InventoryItems = item.InventoryItems.Values.ToList()
                    };
                    await repository.WriteDetails(inventory);
                },
                stoppingToken
            );
        }
    }
}
