using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommunicationModule;

public class KafkaConsumer<TKey, TValue> : IMessageConsumer<TKey, TValue>, IDisposable
{
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;

    public KafkaConsumer(IOptions<KafkaConsumerConfig> config, ILogger<KafkaConsumer<TKey, TValue>> logger, IConfiguration configuration)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = config.Value.BootstrapServers,
            GroupId = config.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<TKey, TValue>(consumerConfig).Build();
        _logger = logger;
    }

    public async Task ConsumeAsync(string topic, Func<TKey, TValue, Task> messageHandler, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10);

                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult != null)
                {
                    _logger.LogInformation($"Received message from Kafka. Key: {consumeResult.Message.Key}, Value: {consumeResult.Message.Value}");

                    await messageHandler(consumeResult.Message.Key, consumeResult.Message.Value);

                    _consumer.Commit(consumeResult);
                }
            }
        }
        catch (ConsumeException ex)
        {
            _logger.LogError($"Error consuming message from Kafka: {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Kafka consuming error: {ex}");
        }
    }

    public void Dispose()
    {
        _consumer?.Close();
        _consumer?.Dispose();
    }
}
