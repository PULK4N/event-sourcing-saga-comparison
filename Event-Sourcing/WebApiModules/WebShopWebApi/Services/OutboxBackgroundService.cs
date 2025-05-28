using CommunicationModule.Interfaces;
using EventSourcing.Core;
using EventSourcing.Core.Interfaces;
using EventSourcing.Shared.Models;

namespace WebShopWebApi.Services;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IMessageProducer<ISharedStateData> _messageProducer;
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OutboxBackgroundService(
        IMessageProducer<ISharedStateData> messageProducer,
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
            await Task.Delay(100);

            using var scope = _serviceProvider.CreateScope();
            var eventStoreWithOutbox = scope.ServiceProvider.GetService<IEventStoreWithOutbox>();
            var steteMachineHandler = scope.ServiceProvider.GetService<StateMachineHandler>();
            if (eventStoreWithOutbox is null)
                throw new Exception("Unable to instantiate event store");

            var message = await eventStoreWithOutbox.GetLatestMessage();
            if (message is null)
                continue;
            var eventsDict = await eventStoreWithOutbox.GetEventsByAggregate(
                message.Payload.EventExecutionInfo.AggregateId
            );
            var events = eventsDict.SelectMany(x => x.Value);
            if (!events.Any())
                continue;

            try
            {
                var stateInfo = await steteMachineHandler.Calculate(events);

                if (stateInfo.StateData is not ISharedStateData stateData)
                    continue;
                await _messageProducer.ProduceAsync(
                    message.Payload.EventExecutionInfo.StateMachineId,
                    message.Payload.EventExecutionInfo.AggregateId.ToString(),
                    stateData
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
