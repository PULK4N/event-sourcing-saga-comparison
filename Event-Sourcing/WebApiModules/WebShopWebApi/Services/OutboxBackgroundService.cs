using CommunicationModule.Interfaces;
using EventSourcing.Core.Interfaces;
using EventSourcing.Shared.Models;

namespace WebShopWebApi.Services;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IMessageProducer<string, EventPayload> _messageProducer;
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OutboxBackgroundService(
        IMessageProducer<string, EventPayload> messageProducer,
        ILogger<OutboxBackgroundService> logger,
        IServiceProvider serviceProvider
    )
    {
        _messageProducer = messageProducer;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            using var scope = _serviceProvider.CreateScope();
            var eventStoreWithOutbox = scope.ServiceProvider.GetService<IEventStoreWithOutbox>();
            if (eventStoreWithOutbox is null)
                throw new Exception("Unable to instantiate event store");

            var message = await eventStoreWithOutbox.GetLatestMessage();

            if (message is null)
                continue;

            try
            {
                await _messageProducer.ProduceAsync(
                    message.Payload.EventExecutionInfo.StateMachineId,
                    message.Payload.EventExecutionInfo.AssemblyQualifiedEventName,
                    message.Payload
                );

                await eventStoreWithOutbox.UpdateCompleted(message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to produce message {e}");
                await eventStoreWithOutbox.UpdateFailed(message.Id);
            }

            await Task.Delay(10);
        }
    }
}
