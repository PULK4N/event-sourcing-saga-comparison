using System.Text.Json;
using System.Text.Json.Serialization;
using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommunicationModule;

public class KafkaProducer<TValue> : IMessageProducer<TValue>, IDisposable
{
    private readonly IProducer<string, TValue> _producer;
    private readonly ILogger<KafkaProducer<TValue>> _logger;

    public KafkaProducer(
        IOptions<KafkaProducerConfig> config,
        ILogger<KafkaProducer<TValue>> logger
    )
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.Value.BootstrapServers
        };

        var builder = new ProducerBuilder<string, TValue>(producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(new ValueJsonSerializer<TValue>());

        _producer = builder.Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, string key, TValue value)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(
                topic,
                new Message<string, TValue> { Key = key, Value = value }
            );
            _logger.LogInformation(
                $"Message sent to topic {topic}, partition {deliveryResult.Partition}, offset {deliveryResult.Offset}"
            );
        }
        catch (ProduceException<string, TValue> ex)
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
