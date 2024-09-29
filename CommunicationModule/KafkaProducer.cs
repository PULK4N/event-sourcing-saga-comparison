using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CommunicationModule.Interfaces;
using CommunicationModule.Config;

namespace CommunicationModule;

public class KafkaProducer<TKey, TValue> : IMessageProducer<TKey, TValue>, IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

    public KafkaProducer(IOptions<KafkaProducerConfig> config, ILogger<KafkaProducer<TKey, TValue>> logger)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<TKey, TValue>(producerConfig).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value });
            _logger.LogInformation($"Message sent to topic {topic}, partition {deliveryResult.Partition}, offset {deliveryResult.Offset}");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Error producing message to Kafka: {ex.Error.Reason}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Kafka producing error: {ex}");
        }
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
}
