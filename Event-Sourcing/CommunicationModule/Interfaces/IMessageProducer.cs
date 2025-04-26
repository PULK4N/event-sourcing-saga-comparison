namespace CommunicationModule.Interfaces;

public interface IMessageProducer<TKey, TValue>
{
    Task ProduceAsync(string topic, TKey key, TValue value);
}
