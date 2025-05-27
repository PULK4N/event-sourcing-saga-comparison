using System.Text.Json;
using System.Text.Json.Serialization;
using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommunicationModule;

public class KafkaProducer<TKey, TValue> : IMessageProducer<TKey, TValue>, IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

    public KafkaProducer(
        IOptions<KafkaProducerConfig> config,
        ILogger<KafkaProducer<TKey, TValue>> logger
    )
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.Value.BootstrapServers
        };

        var builder = new ProducerBuilder<TKey, TValue>(producerConfig)
            // string → UTF8 bytes
            .SetKeySerializer(new ValueJsonSerializer<TKey>())
            // EventPayload → JSON bytes
            .SetValueSerializer(new ValueJsonSerializer<TValue>());

        _producer = builder.Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(
                topic,
                new Message<TKey, TValue> { Key = key, Value = value }
            );
            _logger.LogInformation(
                $"Message sent to topic {topic}, partition {deliveryResult.Partition}, offset {deliveryResult.Offset}"
            );
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Error producing message to Kafka: {ex.Error.Reason}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Kafka producing error: {ex}");
            throw;
        }
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
}

public class ValueJsonSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext ctx) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}
